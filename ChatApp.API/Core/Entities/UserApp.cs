using ChatApp.API.Core.Entities;

namespace ChatApp.API.Core.Domain
{
    public class UserApp
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        //public string? Role { get; set; }

        public int RoleId { get; set; } // Foreign key
        public RoleApp Role { get; set; } // Navigation property
    }
}
