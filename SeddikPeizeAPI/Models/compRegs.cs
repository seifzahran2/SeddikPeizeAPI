using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeddikPeizeAPI.Models
{
    public class compRegs
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Required]
        [MaxLength(100, ErrorMessage = "The Name cannot be more than 100 characters")]
        public string FullName { get; set; }


        [Required]
        [MaxLength(14, ErrorMessage = "The NationalId cannot be more than 14 characters")]
        [MinLength(14, ErrorMessage = "The NationalId cannot be less than 14 characters")]
        public string NationalId { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "The Tittle cannot be more than 100 characters")]

        public string Address { get; set; }


        [Required]
        [MaxLength(50, ErrorMessage = "The Specialization cannot be more than 100 characters")]
        public string Specialization { get; set; }

        [Required]
        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }
        [Required]
        [MaxLength(11)]
        [MinLength(11, ErrorMessage = "The Phone Number cannot be less than 11 Number")]
        [Phone]
        public string MobileNumber { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public int BankAccount { get; set; }

        [Required(ErrorMessage = "BirthDate is required.")]
        public int Age { get; set; }

        public bool IsProjSent { get; set; }
        // هنستخدمها لما نيجي نظهؤ النتيجه
        public bool IsAccepted { get; set; }
        public string Result { get; set; }
        public Projects CompProj { get; set; }
    }
}
