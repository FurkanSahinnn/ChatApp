using ChatApp.API.Core.Application.Constants;
using ChatApp.API.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.API.Persistence.Configurations
{
    public class UserAppConfiguration : IEntityTypeConfiguration<UserApp>
    {
        public void Configure(EntityTypeBuilder<UserApp> builder)
        {
            builder.HasOne(u => u.Role)
               .WithMany(r => r.Users)
               .HasForeignKey(u => u.RoleId)
               .OnDelete(DeleteBehavior.Restrict);

            // Varsayılan kullanıcı örneği
            builder.HasData(
                new UserApp { Id = 1, Name = "Default Admin", Email = "admin@default.com", Password = PasswordHasher.Hash("password"), RoleId = 1 }
            );
            //builder.HasOne(x => x.RoleApp).WithMany(x => x.UserApp).HasForeignKey(x => x.RoleAppId);
        }
    }
}
