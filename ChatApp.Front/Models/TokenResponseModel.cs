namespace ChatApp.Front.Models
{
    public class TokenResponseModel
    {
        public TokenResponseModel(string token, DateTime tokenExpiration)
        {
            Token = token;
            TokenExpiration = tokenExpiration;
        }

        public string Token { get; set; }
        public DateTime TokenExpiration { get; set; }
    }
}
