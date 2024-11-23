using ChatApp.API.Core.Application.Constants;
using ChatApp.API.Core.Application.Features.CQRS.Commands;
using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Domain;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Handlers
{
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommandRequest>
    {
        private readonly IGenericRepository<UserApp> _repository;

        public RegisterUserCommandHandler(IGenericRepository<UserApp> repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(RegisterUserCommandRequest request, CancellationToken cancellationToken)
        {
            await _repository.CreateAsync(new UserApp
            {
                Name = request.UserName,
                Password = request.Password,
                Email = request.Email,
                RoleAppId = RoleTypes.Member,

            });

            return Unit.Value;
        }
    }
}
