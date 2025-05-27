using ChatApp.API.Core.Domain;
using ChatApp.API.Core.Entities;
using ChatApp.API.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace ChatApp.API.Persistence.Context
{
    public class AuthenticationContext : DbContext
    {
        public AuthenticationContext(DbContextOptions<AuthenticationContext> options) : base(options)
        {
        }

        public DbSet<UserApp> UserApps => Set<UserApp>();
        //public DbSet<RoleApp> RoleApps { get { return this.Set<RoleApp>(); } }
        public DbSet<RoleApp> RoleApps => Set<RoleApp>();
        public DbSet<Chat> Chats => Set<Chat>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserAppConfiguration());
            modelBuilder.ApplyConfiguration(new RoleAppConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
