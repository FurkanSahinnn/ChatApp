namespace ChatApp.API.Core.Application.Dtos
{
    public class TokenResponseDto
    {
        public TokenResponseDto(string token, DateTime tokenExpiration)
        {
            Token = token;
            TokenExpiration = tokenExpiration;
        }

        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
