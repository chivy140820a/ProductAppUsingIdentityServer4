using MailKit.Security;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using ProductApp.IdentityProvider.Catalog;

namespace ProductApp.IdentityProvider.SerVice
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings mailSettings;

        private readonly ILogger<EmailSender> logger;


        // mailSetting được Inject qua dịch vụ hệ thống
        // Có inject Logger để xuất log
        public EmailSender(IOptions<MailSettings> _mailSettings, ILogger<EmailSender> _logger)
        {
            mailSettings = _mailSettings.Value;
            logger = _logger;
            logger.LogInformation("Create SendMailService");
        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var emailcontent = new MimeMessage();
            emailcontent.Sender = new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail);
            emailcontent.From.Add(new MailboxAddress(mailSettings.DisplayName, mailSettings.Mail));
            emailcontent.To.Add(MailboxAddress.Parse(email));
            emailcontent.Subject = subject;


            var builder = new BodyBuilder();
            builder.HtmlBody = htmlMessage;
            emailcontent.Body = builder.ToMessageBody();

            // dùng SmtpClient của MailKit
            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            try
            {
                smtp.Connect(mailSettings.Host, mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(mailSettings.Mail, mailSettings.Password);
                await smtp.SendAsync(emailcontent);
            }
            catch (Exception ex)
            {
                // Gửi mail thất bại, nội dung email sẽ lưu vào thư mục mailssave
                System.IO.Directory.CreateDirectory("mailssave");
                var emailsavefile = string.Format(@"mailssave/{0}.eml", Guid.NewGuid());
                await emailcontent.WriteToAsync(emailsavefile);

                logger.LogInformation("Lỗi gửi mail, lưu tại - " + emailsavefile);
                logger.LogError(ex.Message);
            }

            smtp.Disconnect(true);

        }
    }
}
