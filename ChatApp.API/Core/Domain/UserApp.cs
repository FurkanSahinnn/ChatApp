namespace ChatApp.API.Core.Domain
{
    public class UserApp
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public int RoleAppId { get; set; }

        public RoleApp RoleApp { get; set; }

        public UserApp()
        {
            RoleApp = new RoleApp();
        }
    }
}
