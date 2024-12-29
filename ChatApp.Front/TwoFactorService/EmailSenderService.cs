using Microsoft.Extensions.Options;
using SendGrid.Helpers.Mail;
using SendGrid;

namespace ChatApp.Front.TwoFactorService
{
    public class EmailSenderService
    {
        private readonly TwoFactorOptions _twoFactorOptions;

        public EmailSenderService(IOptions<TwoFactorOptions> twoFactorOptions)
        {
            _twoFactorOptions = twoFactorOptions.Value;
        }
        private async Task Execute(string email, string code)
        {
            var apiKey = _twoFactorOptions.SendGrid_ApiKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress("jedimaster610@gmail.com");
            var subject = "Two Factor Authentication";
            var to = new EmailAddress(email);
            //var plainTextContent = "and easy to do anywhere, even with C#";
            var htmlContent = $"<h2>ChatApp Website Validation Code:</h2>{code}<h3></h3>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, null, htmlContent);
            var response = await client.SendEmailAsync(msg);
        }

        public int GetRandomCode()
        {
            Random random = new Random();
            return random.Next(10000, 99999); // 5 haneli random kod üretir.
        }
        public string Send(string emailAddress)
        {
            string generatedRandomCode = GetRandomCode().ToString();
            Execute(emailAddress, generatedRandomCode);
            return generatedRandomCode;
        }

        public int TimeLeft(HttpContext context)
        {
            if (context.Session.GetString("currentTime") == null)
            {
                context.Session.SetString("currentTime", DateTime.Now.AddSeconds(_twoFactorOptions.CodeTimeExpire).ToString());
            }
            DateTime currentTime = DateTime.Parse(context.Session.GetString("currentTime").ToString());
            
            int timeLeft = (int) (currentTime - DateTime.Now).TotalSeconds;
            if (timeLeft <= 0)
            {
                context.Session.Remove("currentTime");
                return 0;
            }
            return timeLeft;

        }
        
        

    }
}
