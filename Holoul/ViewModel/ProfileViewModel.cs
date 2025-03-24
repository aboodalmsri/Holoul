using System.ComponentModel.DataAnnotations;

namespace Holoul.Models
{
    public class ProfileViewModel
    {
        [Required(ErrorMessage = "الاسم الأول مطلوب")]
        [Display(Name = "الاسم الأول")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "الاسم الأخير مطلوب")]
        [Display(Name = "الاسم الأخير")]
        public string LastName { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

    }
}
