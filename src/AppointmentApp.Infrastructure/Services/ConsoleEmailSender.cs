using AppointmentApp.Core.Services;
using System;
using System.Threading.Tasks;

namespace AppointmentApp.Infrastructure.Services
{
    /// <summary>
    /// IEmailSender’ın mock implementasyonu.
    /// Gönderilen e-posta bilgilerini konsola yazar.
    /// Artık kullanılmıyor!!
    /// </summary>
    public class ConsoleEmailSender : IEmailSender
    {
        public Task SendEmailAsync(string to, string subject, string body)
        {
            Console.WriteLine("---- Mock Email Gönderimi ----");
            Console.WriteLine($"To:      {to}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine($"Body:    {body}");
            Console.WriteLine("-----------------------------");
            return Task.CompletedTask;
        }
    }
}
