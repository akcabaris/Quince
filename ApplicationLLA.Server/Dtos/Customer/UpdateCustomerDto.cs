using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Customer
{
    public class UpdateCustomerDto
    {
        [MaxLength(50)]
        public required string FullName { get; set; }

        public required Int64 PhoneNumber { get; set; }

        [MaxLength(16)]
        public string? Gender { get; set; }
    }
}
