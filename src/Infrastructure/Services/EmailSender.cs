using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace CleanArchitecture.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly MailSettings _mailSettings;
        private IApplicationDbContext _context;
        private IEmailTemplate _emailTemplate;
        public EmailSender(IOptions<MailSettings> mailSettings, IApplicationDbContext context, IEmailTemplate emailTemplate)
        {
            _context = context;
            _emailTemplate = emailTemplate;
            _mailSettings = mailSettings.Value;
        }
        public async Task Send(string? content, string? subject,string? to)
        {
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                email.To.Add(MailboxAddress.Parse(to));
                email.Subject = subject;
                var builder = new BodyBuilder { HtmlBody = content };
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                await smtp.AuthenticateAsync(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                await smtp.DisconnectAsync(true);
        }

        public async Task SendEmailConfirmation(Guid userId, string email)
        {
            CancellationToken cancellationToken = CancellationToken.None;
            Domain.Entities.ConfirmEmail emailConfirm = new()
            {
                UserId = userId, CreatedAt = DateTime.UtcNow, ExpiresAt = DateTime.UtcNow.AddMinutes(15)
            };
            await _context.ConfirmEmails.AddAsync(emailConfirm, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            await Task.Run(async () =>
            {
                string content = _emailTemplate.GetTemplate("confirmEmail")
                    .Replace("{link}", emailConfirm.Id.ToString());
                await Send(content, "Confirm Email", email);
            }, cancellationToken);
        }

        public string? GetMail()
        {
            return _mailSettings.Mail;
        }
    }
}