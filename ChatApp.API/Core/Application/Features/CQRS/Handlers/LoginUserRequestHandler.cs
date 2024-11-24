using ChatApp.API.Core.Application.Constants;
using ChatApp.API.Core.Application.Dtos;
using ChatApp.API.Core.Application.Features.CQRS.Queries;
using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Domain;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Handlers
{
    public class LoginUserRequestHandler : IRequestHandler<LoginUserQueryForRequest, LoginUserResponseDto>
    {
        private readonly IGenericRepository<UserApp> _repositoryUserApp;
        //private readonly IGenericRepository<RoleApp> _repositoryRoleApp;

        public LoginUserRequestHandler(IGenericRepository<UserApp> repositoryUserApp)
        {
            _repositoryUserApp = repositoryUserApp;
            //_repositoryRoleApp = repositoryRoleApp;
        }
        public async Task<LoginUserResponseDto> Handle(LoginUserQueryForRequest request, CancellationToken cancellationToken)
        {
            var userAppResponseDto = new LoginUserResponseDto();
            var user = await _repositoryUserApp.WhereAsync(x => x.Email == request.Email);
            
            //var user = await _repositoryUserApp.WhereAsync(x => x.Email == request.Email && x.Password == request.Password);
            if (user == null)
            {
                userAppResponseDto.IsUserAvailable = false;
            } else
            {
                var isPasswordValid = PasswordHasher.Verify(request.Password, user.Password);
                if (isPasswordValid)
                {
                    userAppResponseDto.Id = user.Id;
                    userAppResponseDto.UserName = user.Name;
                    userAppResponseDto.Email = user.Email;
                    userAppResponseDto.Role = user.Role;
                    //var roleName = await _repositoryRoleApp.WhereAsync(x => x.Id == user.RoleAppId);
                    //userAppResponseDto.Role = roleName?.RoleName;

                    userAppResponseDto.IsUserAvailable = true;
                } else
                {
                    userAppResponseDto.IsUserAvailable = false;
                }

            }
            return userAppResponseDto;
        }
    }
}
