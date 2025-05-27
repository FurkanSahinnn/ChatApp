using ChatApp.API.Core.Application.Constants;
using ChatApp.API.Core.Application.Dtos;
using ChatApp.API.Core.Application.Features.CQRS.Queries;
using ChatApp.API.Core.Application.Interfaces;
using ChatApp.API.Core.Domain;
using ChatApp.API.Core.Entities;
using MediatR;

namespace ChatApp.API.Core.Application.Features.CQRS.Handlers
{
    public class LoginUserRequestHandler : IRequestHandler<LoginUserQueryForRequest, LoginUserResponseDto>
    {
        private readonly IGenericRepository<UserApp> _repositoryUserApp;
        private readonly IGenericRepository<RoleApp> _repositoryRoleApp;

        public LoginUserRequestHandler(IGenericRepository<UserApp> userRepository, IGenericRepository<RoleApp> roleRepository)
        {
            _repositoryUserApp = userRepository;
            _repositoryRoleApp = roleRepository;
        }
        public async Task<LoginUserResponseDto> Handle(LoginUserQueryForRequest request, CancellationToken cancellationToken)
        {
            var userAppResponseDto = new LoginUserResponseDto();

            // Kullanıcıyı email üzerinden kontrol et
            var user = await _repositoryUserApp.WhereAsync(x => x.Email == request.Email);

            if (user == null)
            {
                userAppResponseDto.IsUserAvailable = false; // Kullanıcı bulunamadı
            }
            else
            {
                // Şifre doğrulaması
                var isPasswordValid = PasswordHasher.Verify(request.Password, user.Password);
                if (isPasswordValid)
                {
                    // Kullanıcının rolünü RoleApp tablosundan al
                    var role = await _repositoryRoleApp.GetByIdAsync(user.RoleId);

                    userAppResponseDto.Id = user.Id;
                    userAppResponseDto.UserName = user.Name;
                    userAppResponseDto.Email = user.Email;
                    userAppResponseDto.Role = role.Name ?? "Unknown"; // Rol bilgisi yoksa "Unknown"

                    userAppResponseDto.IsUserAvailable = true; // Kullanıcı geçerli
                }
                else
                {
                    userAppResponseDto.IsUserAvailable = false; // Şifre hatalı
                }
            }

            return userAppResponseDto;
        }
    }
}
