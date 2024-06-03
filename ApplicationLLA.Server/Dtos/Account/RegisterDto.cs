using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Account
{
    public class RegisterDto
    {
        [Required]
        public string? Username { get; set; }
        [EmailAddress]
        public required string? Email { get; set; }
        public required string? Password { get; set; }
        public required string AccountType { get; set; }
    }
}
