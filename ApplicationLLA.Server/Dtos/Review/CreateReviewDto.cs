using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Review
{
    public class CreateReviewDto
    {
        [MaxLength(240)]
        public required string ReviewText { get; set; }
        [Range(1, 5, ErrorMessage = "Review score can be just integer 1 to 5")]
        public required int ReviewScore { get; set; }
        public required int ReservationId { get; set; }
        [MaxLength(64)]
        public required string ReviewToUserId { get; set; }
    }
}
