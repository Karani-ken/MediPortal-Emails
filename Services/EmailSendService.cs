using MediPortal_Emails.Models.Dtos;
using MimeKit;
using MailKit.Net.Smtp;

namespace MediPortal_Emails.Services
{
    public class EmailSendService
    {
        private readonly string email;
        private readonly string password;
        public EmailSendService(IConfiguration _configuration)
        {

            email = _configuration.GetSection("EmailService:Email").Get<string>();
            password = _configuration.GetSection("EmailService:Password").Get<string>();
        }

        public async Task SendMail(EmailMessage messageDto, string message)
        {
            MimeMessage message1 = new MimeMessage();
            message1.From.Add(new MailboxAddress("MediPortal", email));

            //send the recipient address
            message1.To.Add(new MailboxAddress(messageDto.Name, messageDto.Email));

            message1.Subject = "MediPortal online hospital";
            var body = new TextPart("html")
            {
                Text = message.ToString()
            };
            message1.Body = body;

            var client = new SmtpClient();
            client.Connect("smtp.gmail.com", 587, false);

            client.Authenticate(email, password);

            await client.SendAsync(message1);
            await client.DisconnectAsync(true);
        }
    }
}
