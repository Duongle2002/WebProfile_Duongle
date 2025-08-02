using System.Net.Mail;
using System.Net;
using MyWebProfile.Models;
using Microsoft.EntityFrameworkCore;

namespace MyWebProfile.Services
{
    public interface IEmailService
    {
        Task<bool> SendContactNotificationAsync(ContactMessage contactMessage);
        Task<bool> SendEmailAsync(string to, string subject, string body);
    }

    public class EmailService : IEmailService
    {
        private readonly MyWebProfileContext _context;

        public EmailService(MyWebProfileContext context)
        {
            _context = context;
        }

        public async Task<bool> SendContactNotificationAsync(ContactMessage contactMessage)
        {
            try
            {
                var emailSettings = await _context.EmailSettings.FirstOrDefaultAsync();
                if (emailSettings == null || !emailSettings.EnableEmailNotification)
                {
                    return false;
                }

                var subject = $"Tin nhắn liên hệ mới từ {contactMessage.Name}";
                var body = GenerateContactEmailBody(contactMessage);

                return await SendEmailAsync(emailSettings.ToEmail, subject, body);
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var emailSettings = await _context.EmailSettings.FirstOrDefaultAsync();
                if (emailSettings == null)
                {
                    return false;
                }

                using var client = new SmtpClient(emailSettings.SmtpServer, emailSettings.SmtpPort)
                {
                    EnableSsl = emailSettings.EnableSsl,
                    Credentials = new NetworkCredential(emailSettings.FromEmail, emailSettings.EmailPassword)
                };

                var message = new MailMessage
                {
                    From = new MailAddress(emailSettings.FromEmail, emailSettings.FromName),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                message.To.Add(to);

                await client.SendMailAsync(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private string GenerateContactEmailBody(ContactMessage contactMessage)
        {
            return $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                        .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                        .header {{ background: linear-gradient(135deg, #667eea 0%, #764ba2 100%); color: white; padding: 20px; border-radius: 10px 10px 0 0; }}
                        .content {{ background: #f8f9fa; padding: 20px; border-radius: 0 0 10px 10px; }}
                        .field {{ margin-bottom: 15px; }}
                        .label {{ font-weight: bold; color: #667eea; }}
                        .value {{ margin-top: 5px; }}
                        .message {{ background: white; padding: 15px; border-radius: 8px; border-left: 4px solid #667eea; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>📧 Tin nhắn liên hệ mới</h2>
                            <p>Bạn có tin nhắn liên hệ mới từ website</p>
                        </div>
                        <div class='content'>
                            <div class='field'>
                                <div class='label'>👤 Họ và tên:</div>
                                <div class='value'>{contactMessage.Name}</div>
                            </div>
                            <div class='field'>
                                <div class='label'>📧 Email:</div>
                                <div class='value'>{contactMessage.Email}</div>
                            </div>
                            {(string.IsNullOrEmpty(contactMessage.Phone) ? "" : $@"
                            <div class='field'>
                                <div class='label'>📞 Số điện thoại:</div>
                                <div class='value'>{contactMessage.Phone}</div>
                            </div>")}
                            {(string.IsNullOrEmpty(contactMessage.Subject) ? "" : $@"
                            <div class='field'>
                                <div class='label'>📝 Chủ đề:</div>
                                <div class='value'>{contactMessage.Subject}</div>
                            </div>")}
                            <div class='field'>
                                <div class='label'>💬 Nội dung tin nhắn:</div>
                                <div class='message'>{contactMessage.Message}</div>
                            </div>
                            <div class='field'>
                                <div class='label'>⏰ Thời gian:</div>
                                <div class='value'>{contactMessage.CreatedAt:dd/MM/yyyy HH:mm}</div>
                            </div>
                        </div>
                    </div>
                </body>
                </html>";
        }
    }
} 