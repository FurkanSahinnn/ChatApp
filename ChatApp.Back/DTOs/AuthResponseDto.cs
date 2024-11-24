namespace ChatApp.Back.DTOs
{
    public class AuthResponseDto
    {
        public bool isSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
