using ApplicationLLA.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Worker
{
    public class UpdateWorkerPicDto
    {
        [Required]
        public IFormFile? ImageFile { get; set; }

    }
}
