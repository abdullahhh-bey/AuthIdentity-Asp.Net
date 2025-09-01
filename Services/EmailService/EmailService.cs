using MailKit.Net.Smtp;
using MailKit;
using MimeKit;

namespace UserAuthManagement.Services.EmailService
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration; 
        }

        //Function to send emails 
        public async Task SendEmailsAsync(string toEmail , string subject, string body)
        {
            var message = new MimeMessage();

            message.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:From"]));
            message.To.Add(MailboxAddress.Parse(toEmail));
            message.Subject = subject;
            message.Body = new TextPart("plain")
            {
                Text = body
            };


            //Connext to SMTP Server through smtp client
            var smtp = new SmtpClient();
            await smtp.ConnectAsync(
                _configuration["EmailSettings:SmtpServer"],
                int.Parse(_configuration["EmailSettings:Port"] ?? "465"),
                true); // true = SSL
            await smtp.AuthenticateAsync(
                _configuration["EmailSettings:Username"],
                _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
        

    }
}
