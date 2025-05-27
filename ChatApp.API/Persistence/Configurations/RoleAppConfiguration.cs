using ChatApp.API.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ChatApp.API.Persistence.Configurations
{
    public class RoleAppConfiguration : IEntityTypeConfiguration<RoleApp>
    {
        public void Configure(EntityTypeBuilder<RoleApp> builder)
        {
            builder.HasKey(r => r.Id); // Primary key
            builder.Property(r => r.Name).IsRequired().HasMaxLength(50);

            // Varsayılan rollerin oluşturulması
            builder.HasData(
                new RoleApp { Id = 1, Name = "Admin" },
                new RoleApp { Id = 2, Name = "Member" }
            );
        }
    }
}
