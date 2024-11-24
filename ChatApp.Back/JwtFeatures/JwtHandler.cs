namespace ChatApp.Back.JwtFeatures
{
    public class JwtHandler
    {
        private readonly IConfiguration _configuration;
        

        public JwtHandler(IConfiguration configuration)
        {
            _configuration = configuration;
            
        }
    }
}
