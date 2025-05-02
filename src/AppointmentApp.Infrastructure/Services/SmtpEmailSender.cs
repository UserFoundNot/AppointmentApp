using MailKit.Security;
using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Options;
using AppointmentApp.Core.Services;
using AppointmentApp.Infrastructure.Services;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System;

public class SmtpEmailSender : IEmailSender
{
    private readonly EmailSettings _opts;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(
        IOptions<EmailSettings> opts,
        ILogger<SmtpEmailSender> logger)
    {
        _opts = opts.Value;
        _logger = logger;
    }

    public async Task SendEmailAsync(string to, string subject, string body)
    {
        try
        {
            var msg = new MimeMessage();
            msg.From.Add(MailboxAddress.Parse(_opts.From));
            msg.To.Add(MailboxAddress.Parse(to));
            msg.Subject = subject;
            msg.Body = new TextPart("html") { Text = body };

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(_opts.Host, _opts.Port, SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_opts.Username, _opts.Password);
            await client.SendAsync(msg);
            await client.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            // Hata logla, akışa devam et
            _logger.LogError(ex, "E-posta gönderilemiyor: {To}, Subject: {Subject}", to, subject);
        }
    }
}

