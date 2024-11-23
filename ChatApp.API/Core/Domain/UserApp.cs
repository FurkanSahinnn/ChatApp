namespace ChatApp.API.Core.Domain
{
    public class UserApp
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }

        public string? Password { get; set; }

        public int RoleAppId { get; set; } = 2;

        public RoleApp RoleApp { get; set; }
        /*
        public UserApp()
        {
            RoleApp = new RoleApp();
        }
        */
    }
}
