using ChatApp.API.Core.Application.Features.CQRS.Queries;
using ChatApp.API.Core.Application.Options;
using ChatApp.API.JwtFeatures;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ChatApp.API.Controllers
{
    // [Authorize(Roles="Admin")] --> The Auth can be seen from admin.
    // [Authorize(Roles="Admin,Member")] --> The Auth can be seen from admin and members.
    
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
        public async Task<IActionResult> Register(RegisterUserQueryForRequest request)
        {
            var dto = await _mediator.Send(request);

            if (dto.IsExist)
            {
                return BadRequest("Username or Email is already used.");
            }
            
            return Created("", request);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginUserQueryForRequest request)
        {
            var dto = await _mediator.Send(request);
            if (dto.IsUserAvailable)
            {
                var createdToken = TokenGenerator.CreateToken(_options, dto);
                return Created("", createdToken);
            }
            return Unauthorized("Invalid Authentication.");
        }
    }
}
