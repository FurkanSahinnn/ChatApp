using ChatApp.API.Core.Domain;
using ChatApp.API.Core.Entities;
using ChatApp.API.Persistence.Context;
using Microsoft.AspNetCore.SignalR;

namespace ChatApp.API.Core.Hubs
{
    public class ChatHub(AuthenticationContext context) : Hub
    {
        public static Dictionary<string, int> Users = new();
        
        public async Task Connect(int UserId)
        {
            Users.Add(Context.ConnectionId, UserId);
            UserApp? user = await context.UserApps.FindAsync(UserId);
            if (user != null)
            {
                await context.SaveChangesAsync();
                await Clients.All.SendAsync("Users", user);
            }
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            int UserId;
            Users.TryGetValue(Context.ConnectionId, out UserId);
            Users.Remove(Context.ConnectionId);
            UserApp? user = await context.UserApps.FindAsync(UserId);
            if (user != null)
            {
                await context.SaveChangesAsync();
                await Clients.All.SendAsync("Users", user);
            }
        }
    }
}
