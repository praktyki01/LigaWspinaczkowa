using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace LigaWspinaczkowa.Services
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string emailto, string subject, string htmlMessage)
        {
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Liga Wspinaczkowa", "ligawspinaczkowa@wp.pl"));
            email.To.Add(MailboxAddress.Parse(emailto));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlMessage };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.wp.pl", 587, SecureSocketOptions.Auto);
            smtp.Authenticate("ligawspinaczkowa@wp.pl", "Haslo1!!");
            smtp.Send(email);
            smtp.Disconnect(true);

            return Task.CompletedTask;
        }
    }
}
