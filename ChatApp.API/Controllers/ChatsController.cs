using ChatApp.API.Core.Application.Dtos;
using ChatApp.API.Core.Domain;
using ChatApp.API.Core.Entities;
using ChatApp.API.Core.Hubs;
using ChatApp.API.Persistence.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class ChatsController(
        AuthenticationContext context,
        IHubContext<ChatHub> hubContext) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> GetChats(int UserId, int ToUserId, CancellationToken cancellationToken)
        {
            List<Chat> chats = await context.Chats.Where(p => p.UserId == UserId && p.ToUserId == ToUserId ||
            p.ToUserId == UserId && p.UserId == ToUserId)
                .OrderBy(p => p.Date)
                .ToListAsync(cancellationToken);
            return Ok(chats);
        }

        [HttpPost]
        public async Task<IActionResult> SendMessage(SendMessageDto request, CancellationToken cancellationToken)
        {
            Chat chat = new()
            {
                UserId = request.UserId,
                ToUserId = request.ToUserId,
                Message = request.Message,
                Date = DateTime.UtcNow
            };
            await context.AddAsync(chat, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            
            string connectionId = ChatHub.Users.FirstOrDefault(p => p.Value == chat.ToUserId).Key;
            
            if (!string.IsNullOrEmpty(connectionId))
            {
                await hubContext.Clients.Client(connectionId).SendAsync("Messages", chat);
            }
            
            return Ok(chat);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(int? currentUserId)
        {
            if (!currentUserId.HasValue)
            {
                return BadRequest("Current user ID cannot be null.");
            }

            List<UserApp> users = await context.UserApps
                .Where(p => p.Id != currentUserId.Value)
                .OrderBy(p => p.Name)
                .ToListAsync();

            foreach (var user in users)
            {
                user.Password = null; // Şifreyi gizle.
            }

            return Ok(users);
        }
    }
}
