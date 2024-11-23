using ChatApp.API.Core.Application.Dtos;
using ChatApp.API.Core.Application.Features.CQRS.Queries;
using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Domain;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Handlers
{
    public class CheckUserRequestHandler : IRequestHandler<CheckUserQueryForRequest, CheckUserResponseDto>
    {
        private readonly IGenericRepository<UserApp> _repositoryUserApp;
        private readonly IGenericRepository<RoleApp> _repositoryRoleApp;

        public CheckUserRequestHandler(IGenericRepository<UserApp> repositoryUserApp, IGenericRepository<RoleApp> repositoryRoleApp)
        {
            _repositoryUserApp = repositoryUserApp;
            _repositoryRoleApp = repositoryRoleApp;
        }
        public async Task<CheckUserResponseDto> Handle(CheckUserQueryForRequest request, CancellationToken cancellationToken)
        {
            var userAppResponseDto = new CheckUserResponseDto();

            var user = await _repositoryUserApp.WhereAsync(x => x.Name == request.Username && x.Password == request.Password);
            if (user == null)
            {
                userAppResponseDto.IsUserAvailable = false;
            } else
            {
                userAppResponseDto.Id = user.Id;
                userAppResponseDto.UserName = user.Name;
                var roleName = await _repositoryRoleApp.WhereAsync(x => x.Id == user.RoleAppId);
                userAppResponseDto.Role = roleName?.RoleName;


                userAppResponseDto.IsUserAvailable = true;
            }
            return userAppResponseDto;
        }
    }
}
