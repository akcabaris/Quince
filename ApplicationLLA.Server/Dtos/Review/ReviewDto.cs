using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Review
{
    public class ReviewDto
    {
        public int ReviewId { get; set; }
        [MaxLength(240)]
        public required string ReviewText { get; set; }
        [Range(1, 5, ErrorMessage = "Review score can be just integer 1 to 5")]
        public int ReviewScore { get; set; }
        public int ReservationId { get; set; }
        [MaxLength(64)]
        public required string ReviewToUserId { get; set; }
        [MaxLength(64)]
        public required string ReviewWriterUserId { get; set; }
    }
}
