using ChatApp.API.Core.Application.Features.CQRS.Commands;
using ChatApp.API.Core.Application.Features.CQRS.Queries;
using ChatApp.API.Core.Application.Options;
using ChatApp.API.Infrastructures.Tools;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ChatApp.API.Controllers
{
    // [Authorize(Roles="Admin")] --> The Auth can be seen from admin.
    // [Authorize(Roles="Admin,Member")] --> The Auth can be ssen from admin and members.
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IOptions<CustomTokenOptions> _options;
        public AuthController(IMediator mediator, IOptions<CustomTokenOptions> options)
        {
            _mediator = mediator;
            _options = options;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserCommandRequest request)
        {
            await _mediator.Send(request);
            return Created("", request);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(CheckUserQueryForRequest request)
        {
            var dto = await _mediator.Send(request);
            if (dto.IsUserAvailable)
            {
                var createdToken = TokenGenerator.CreateToken(_options, dto);
                return Created("", createdToken);
            }
            return BadRequest("Username or Password is not correct.");
            
        }
    }
}
