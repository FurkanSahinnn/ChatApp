using ChatApp.API.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.API.Persistence.Configurations
{
    public class UserAppConfiguration : IEntityTypeConfiguration<UserApp>
    {
        public void Configure(EntityTypeBuilder<UserApp> builder)
        {
            //builder.HasOne(x => x.RoleApp).WithMany(x => x.UserApp).HasForeignKey(x => x.RoleAppId);
        }
    }
}
