using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeddikPeizeAPI.Models.ViewModels
{
    public class ProjectsVM
    {
        
        [Required(ErrorMessage = "برجاء ادخال رابط درايف الخاص بك")]
        public string DriveLink { get; set; }
    }
}
