using System.ComponentModel.DataAnnotations;

namespace SeddikPeizeAPI.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "تاكد من ادخال البريد الالكتروني")]
        public string Email { get; set; }

        [Required(ErrorMessage = "تاكد من ادخال الرقم السري")]
        public string Password { get; set; }
    }
}
