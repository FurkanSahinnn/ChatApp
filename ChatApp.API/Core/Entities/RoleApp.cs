using ChatApp.API.Core.Domain;

namespace ChatApp.API.Core.Entities
{
    public class RoleApp
    {
        public int Id { get; set; }
        public string Name { get; set; } = "member";
        public ICollection<UserApp> Users { get; set; }
    }
}
