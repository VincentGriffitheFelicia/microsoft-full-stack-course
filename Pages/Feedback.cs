using System.ComponentModel.DataAnnotations;

namespace FeedbackApp
{
    public class Feedback
    {
        [Required(ErrorMessage = "Please fill out your name.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters.")]
        public string Comment { get; set; }
    }
}