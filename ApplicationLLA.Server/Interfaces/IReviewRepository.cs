using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> CreateReviewAsync(Review reviewModel);
        Task<bool> CheckExistsAsync(string writerUserId, int reservationId);
        Task<Review?> GetReviewForReservationAsync(int reservationId);
        Task<List<Review>> GetWorkersReviewsAsync(string userId);
        Task<bool> DeleteAsync(int reviewId);
        Task<double?> GetWorkersReviewScore(string workerId);
    }
}
