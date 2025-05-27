using ChatApp.API.Core.Application.Dtos;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Queries
{
    public class LoginUserQueryForRequest : IRequest<LoginUserResponseDto>
    {
        //public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
