using ChatApp.API.Core.Domain;
using ChatApp.API.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Persistence.Context
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }

        public DbSet<UserApp> UserApps { get { return this.Set<UserApp>(); } }
        //public DbSet<RoleApp> RoleApps { get { return this.Set<RoleApp>(); } }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAppConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
