using System.ComponentModel.DataAnnotations;

namespace Course_Repository.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "FirstName is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "FirstName must be between 1 and 100 characters")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "LastName is required")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "LastName must be between 1 and 100 characters")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Email must be a valid email address")]
        public string Email { get; set; } = string.Empty;

        [Phone(ErrorMessage = "PhoneNumber must be a valid phone number")]
        [StringLength(20, ErrorMessage = "PhoneNumber cannot exceed 20 characters")]
        public string PhoneNumber { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
