using System.Threading.Tasks;

namespace AppointmentApp.Core.Services
{
    /// <summary>
    /// E-posta gönderim servisinin soyut arayüzü.
    /// </summary>
    public interface IEmailSender
    {
        /// <summary>
        /// Belirtilen alıcıya konu ve gövde ile e-posta gönderir.
        /// </summary>
        /// <param name="to">Alıcı e-posta adresi.</param>
        /// <param name="subject">E-posta konusu.</param>
        /// <param name="body">E-posta içeriği (HTML veya düz metin).</param>
        Task SendEmailAsync(string to, string subject, string body);
    }
}
