using System.ComponentModel.DataAnnotations;

namespace ekart.Models
{
    // Request model for login endpoint
    public class LoginRequest
    {
        [Required]
        public string? Email { get; set; } // User email

        [Required]
        public string? Password { get; set; } // User password
    }
}
