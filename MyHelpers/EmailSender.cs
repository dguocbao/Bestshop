using System.Net.Mail;
using System.Threading.Tasks;
namespace Bestshop.MyHelpers
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmail(string email, string name, string subject, string message)
        {
            // You can use SMTP or a third-party service like SendGrid to send the email.
            var smtpClient = new SmtpClient("smtp.yourserver.com")
            {
                Port = 587,
                Credentials = new System.Net.NetworkCredential("your-email@domain.com", "your-password"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("your-email@domain.com"),
                Subject = subject,
                Body = message,
                IsBodyHtml = false,
            };

            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    }
}
