using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentApp.Core.Entities
{
    /// <summary>
    /// Randevu varlığı.
    /// </summary>
    public class Appointment
    {
        /// <summary>
        /// Birincil anahtar.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Randevu tarihi ve saati.
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Randevu başlığı.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Randevu açıklaması.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Bu randevuyu oluşturan kullanıcıya ait ID.
        /// </summary>
        public string UserId { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual User User { get; set; }

    }
}
