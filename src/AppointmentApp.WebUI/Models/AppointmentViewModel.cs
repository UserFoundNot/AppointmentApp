using System;
using System.ComponentModel.DataAnnotations;

namespace AppointmentApp.WebUI.Models
{
    public class AppointmentViewModel
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Randevu Tarihi")]
        public DateTime Date { get; set; }

        [Required, StringLength(100)]
        [Display(Name = "Başlık")]
        public string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "Açıklama")]
        public string Description { get; set; }
    }
}
