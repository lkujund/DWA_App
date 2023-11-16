using System.Net.Mail;
using System.Security.Policy;

namespace Administration.Services
{
    public interface IMailService
    {
        public void Send(string email, string securityToken);
    }
    public class MailService : IMailService
    {
        public void Send(string email, string securityToken)
        {
            SmtpClient client = new SmtpClient("localhost"); //nuget console -> smtp4dev

            string url = $"http://localhost:5134/UserAdmin/ValidateEmail?email={email}&securitytoken={securityToken}";

            MailAddress from = new MailAddress("test@test.com");
            MailAddress to = new MailAddress(email);
            MailMessage message = new MailMessage(from, to);
            message.Body = $"Please verify your e-mail with the following link: "+url;
            message.Subject = "Verification e-mail";

            client.Send(message);
        }
    }
}
