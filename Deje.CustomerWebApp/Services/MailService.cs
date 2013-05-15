using System.Net.Mail;
using System.Text;

namespace Deje.CustomerWebApp.Services
{
    public class MailService
    {
        public void SendMail(string emailAddress, string subject, string message)
        {
            var smtp = new SmtpClient();
            var mail = new MailMessage("no-reply@deje.com", emailAddress)
            {
                Subject = subject,
                Body = message,
                BodyEncoding = Encoding.UTF8
            };
            smtp.SendAsync(mail, null);
        } 
    }
}