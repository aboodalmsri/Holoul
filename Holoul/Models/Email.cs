using System.ComponentModel.DataAnnotations;

namespace Holoul.Models
{
    public class Email
    {
        [Required(ErrorMessage = "To email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string to { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Body is required")]
        public string Body { get; set; }
    }
}
