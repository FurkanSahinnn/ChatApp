using ChatApp.API.Core.Application.Constants;
using ChatApp.API.Core.Application.Dtos;
using ChatApp.API.Core.Application.Features.CQRS.Queries;
using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Domain;
using ChatApp.API.Core.Entities;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Handlers
{
    public class RegisterUserRequestHandler : IRequestHandler<RegisterUserQueryForRequest, RegisterUserResponseDto>
    {
        private readonly IGenericRepository<UserApp> _userRepository;
        private readonly IGenericRepository<RoleApp> _roleRepository;

        public RegisterUserRequestHandler(IGenericRepository<UserApp> userRepository, IGenericRepository<RoleApp> roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }
        public async Task<RegisterUserResponseDto> Handle(RegisterUserQueryForRequest request, CancellationToken cancellationToken)
        {
            var userAppResponseDto = new RegisterUserResponseDto();
            // Check if user already exists
            var isExist = await _userRepository.WhereAsync(x => x.Email == request.Email);
            if (isExist != null)
            {
                userAppResponseDto.IsExist = true;
                return userAppResponseDto;
            }

            // Check if the "member" role exists
            var memberRole = await _roleRepository.WhereAsync(r => r.Name == RoleTypes.Member.ToString());
            if (memberRole == null)
            {
                // Create the default "member" role if it doesn't exist
                memberRole = new RoleApp { Name = RoleTypes.Member.ToString() };
                await _roleRepository.CreateAsync(memberRole);
            }

            // Hash the user's password
            var hashedPassword = PasswordHasher.Hash(request.Password);

            // Create the new user
            await _userRepository.CreateAsync(new UserApp
            {
                Name = request.UserName,
                Password = hashedPassword,
                Email = request.Email,
                RoleId = memberRole.Id // Assign the "member" role by default
            });

            userAppResponseDto.IsExist = false;
            return userAppResponseDto;
        }
    }
}
