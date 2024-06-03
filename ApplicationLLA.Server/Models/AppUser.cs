using Microsoft.AspNetCore.Identity;

namespace ApplicationLLA.Server.Models
{
    public class AppUser : IdentityUser
    {
        public string? CustomerId { get; set; }

        public string? WorkerId { get; set; }

        public Customer? Customer { get; set; }
        
        public Worker? Worker { get; set; }

    }
}
