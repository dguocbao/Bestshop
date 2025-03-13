namespace Bestshop.MyHelpers
{
    public interface IEmailSender
    {
        Task SendEmail(string email, string name, string subject, string message);
    }
}
