namespace ChatApp.Front.Models
{
    public class UserModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public int RoleId { get; set; }
        public string ProfileImageUrl { get; set; } = "/images/default-profile.png";
    }
}
