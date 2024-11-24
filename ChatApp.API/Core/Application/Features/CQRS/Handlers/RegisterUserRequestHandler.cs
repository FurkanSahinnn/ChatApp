using ChatApp.API.Core.Application.Constants;
using ChatApp.API.Core.Application.Dtos;
using ChatApp.API.Core.Application.Features.CQRS.Queries;
using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Domain;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Handlers
{
    public class RegisterUserRequestHandler : IRequestHandler<RegisterUserQueryForRequest, RegisterUserResponseDto>
    {
        private readonly IGenericRepository<UserApp> _repository;

        public RegisterUserRequestHandler(IGenericRepository<UserApp> repository)
        {
            _repository = repository;
        }
        public async Task<RegisterUserResponseDto> Handle(RegisterUserQueryForRequest request, CancellationToken cancellationToken)
        {
            var userAppResponseDto = new RegisterUserResponseDto();
            var isExist = await _repository.WhereAsync(x => x.Email == request.Email);
           
            if (isExist == null)
            {
                userAppResponseDto.IsExist = false;
                var hashedPassword = PasswordHasher.Hash(request.Password);
                await _repository.CreateAsync(new UserApp
                {
                    Name = request.UserName,
                    Password = hashedPassword, // request.Password
                    Email = request.Email,
                    Role = RoleTypes.Member, // Default
                });
            } else
            {
                userAppResponseDto.IsExist = true;
            }

            return userAppResponseDto;
        }
    }
}
