using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SeddikPeizeAPI.Models.ViewModels
{
    public class RegisterVM
    {
        
        [Required(ErrorMessage = "تاكد من ادخال اسمك")]
        public string Name { get; set; }
        [Required(ErrorMessage = "تاكد من ادخال البريد الالكتروني")]
        public string Email { get; set; }
        [Required(ErrorMessage = "تاكد من ادخال الرقم السري")]
        public string Password { get; set; }
        [Required(ErrorMessage = "تاكد من ادخال تأكيد الرقم السري")]
        [Compare("Password", ErrorMessage = "الرقم السري و تأكيد الرقم السري غير متطابقين")]
        public string ConfPassword { get; set; }
        [Required(ErrorMessage = "تاكد من ادخال عمرك")]
        public int age { get; set; }
        [MaxLength(11, ErrorMessage = "لا يزيد الرقم القومي عن 11 رقم")]
        [MinLength(11, ErrorMessage = "لا يقل الرقم القومي عن 11 رقم")]
        [Required(ErrorMessage = "تاكد من ادخال رقم الهاتف")]
        public string mobileNumber { get; set; }
        [Required(ErrorMessage = "تاكد من ادخال الرقم القومي")]
        [MaxLength(14, ErrorMessage = "لا يزيد الرقم القومي عن 14 رقم")]
        [MinLength(14, ErrorMessage = "لا يقل الرقم القومي عن 14 رقم")]
        public string NationalID { get; set; }
        [Required(ErrorMessage = "تاكد من اختيار النوع ")]
        public string gender { get; set; }
    }
}
