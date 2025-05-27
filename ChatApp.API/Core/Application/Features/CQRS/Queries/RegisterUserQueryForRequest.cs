using ChatApp.API.Core.Application.Dtos;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Queries
{
    public class RegisterUserQueryForRequest : IRequest<RegisterUserResponseDto>
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
