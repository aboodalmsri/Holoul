using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;


namespace Holoul.Models
{
    public class ProblemViewModel
    {
        [Required(ErrorMessage = "الوصف مطلوب.")]
        [Display(Name = "وصف المشكلة")]
        public string ProblemDescription { get; set; }

        [Display(Name = "صورة المشكلة")]
        public IFormFile? ProblemImage { get; set; }

        public string? AiSolution { get; set; } // لعرض حل الذكاء الاصطناعي
    }
}
