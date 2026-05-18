using System.ComponentModel.DataAnnotations;

namespace GLMS.Api.Models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "User";

        public DateTime CreatedAt { get; set; }
    }
}