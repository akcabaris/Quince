using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Customer
{
    public class UpdateCustomerPicDto
    {
        [Required]
        public IFormFile? ImageFile { get; set; }
    }
}
