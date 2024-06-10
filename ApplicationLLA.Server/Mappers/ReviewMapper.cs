using ApplicationLLA.Server.Dtos.Review;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Mappers
{
    public static class ReviewMapper
    {
        public static ReviewDto ToReviewDto(this Review review)
        {
            return new ReviewDto
            {
                ReviewId = review.ReviewId,
                ReviewText = review.ReviewText,
                ReviewToUserId = review.ReviewToUserId,
                ReviewWriterUserId = review.ReviewWriterUserId,
                ReviewScore = review.ReviewScore,
                ReservationId = review.reservationId,
            };
        }

        public static Review ToReviewFromCreateDto(this CreateReviewDto review, string writerUserId, string toUserId)
        {
            return new Review
            {
                ReviewText = review.ReviewText,
                ReviewToUserId = toUserId,
                ReviewWriterUserId = writerUserId,
                ReviewScore = review.ReviewScore,
                reservationId = review.ReservationId,
            };
        }
    }
}
