using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;

namespace IdentityServer
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration) => _configuration = configuration;

        public async Task SendEmailAsync(string email, string subject, string body)
        {
            var emailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["SmtpServer:Login"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };
            emailMessage.To.Add(email);


            var client = new SmtpClient
            {
                Host = _configuration["SmtpServer:Host"],
                Port = _configuration.GetValue<int>("SmtpServer:Port"),
                EnableSsl = _configuration.GetValue<bool>("SmtpServer:EnableSsl"),
                Credentials = new NetworkCredential(_configuration["SmtpServer:Login"],
                    _configuration["SmtpServer:Password"])
            };

            await client.SendMailAsync(emailMessage);
        }
    }
}