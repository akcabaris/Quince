using ApplicationLLA.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Worker
{
    public class UpdateWorkerDto
    {
        [MaxLength(50)]
        [MinLength(3)]
        public required string FullName { get; set; }
        public required Int64 PhoneNumber { get; set; }
        [MaxLength(2000)]
        [MinLength(10)]
        public required string Description { get; set; }
        public DateTime? BirthDate { get; set; }
        [MaxLength(30)]
        [MinLength(4)]
        public required string Occupation { get; set; }

    }
}
