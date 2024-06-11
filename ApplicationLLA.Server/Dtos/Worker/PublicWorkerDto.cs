using ApplicationLLA.Server.Dtos.Review;

namespace ApplicationLLA.Server.Dtos.Worker
{
    public class PublicWorkerDto
    {
        public required string WorkerId { get; set; }
        public required string FullName { get; set; }
        public string? PictureLink { get; set; }
        public required Int64 PhoneNumber { get; set; }
        public string? Description { get; set; }
        public DateTime? BirthDate { get; set; }
        public string? Occupation { get; set; }
        public double? ReviewScore {  get; set; }
        public List<ReviewDto> ReviewList { get; set; } = new List<ReviewDto>();
    }
}
