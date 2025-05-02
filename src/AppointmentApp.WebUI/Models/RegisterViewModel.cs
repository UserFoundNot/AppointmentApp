using System.ComponentModel.DataAnnotations;

namespace AppointmentApp.WebUI.Models
{
    public class RegisterViewModel
    {
        [Required, EmailAddress]
        [Display(Name = "E-posta")]
        public string Email { get; set; }

        [Required, DataType(DataType.Password)]
        [Display(Name = "�ifre")]
        public string Password { get; set; }

        [Required, DataType(DataType.Password), Compare("Password")]
        [Display(Name = "�ifre (Tekrar)")]
        public string ConfirmPassword { get; set; }
    }
}
