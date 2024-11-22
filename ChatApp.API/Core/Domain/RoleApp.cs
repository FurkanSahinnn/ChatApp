namespace ChatApp.API.Core.Domain
{
    public class RoleApp
    {
        public int Id { get; set; }
        public string? RoleName { get; set; }

        public List<UserApp> UserApp { get; set; }
        public RoleApp()
        {
            UserApp = new List<UserApp>();
        }
    }
}
