using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        [MaxLength(20)]
        [MinLength(6)]
        public required string Username { get; set; }
        [EmailAddress]
        [MaxLength(50)]
        [MinLength(5)]
        public required string Email { get; set; }
        [MaxLength(20)]
        [MinLength(12)]
        public required string Password { get; set; }
        public required string AccountType { get; set; }
    }
}
