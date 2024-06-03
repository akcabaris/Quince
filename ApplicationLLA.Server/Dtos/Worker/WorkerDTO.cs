using ApplicationLLA.Server.Models;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Worker
{
    public class WorkerDto
    {
        public required string WorkerId { get; set; }
        public required string FullName { get; set; }
        public string? PictureLink { get; set; }
        public required Int64 PhoneNumber { get; set; }
        public string? Description { get; set; }

        public DateTime? BirthDate { get; set; }
        public string? Occupation { get; set; }



    }
}
