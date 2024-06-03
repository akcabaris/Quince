using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Customer
{
    public class CustomerDto
    {
        public required string CustomerId { get; set; }
        [StringLength(254)]
        public string? PictureLink { get; set; }


        [StringLength(40)]
        public required string FullName { get; set; }

        public required Int64 PhoneNumber { get; set; }


        [MaxLength(16)]
        public string? Gender { get; set; }
    }
}
