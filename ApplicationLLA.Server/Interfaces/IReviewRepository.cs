using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IReviewRepository
    {
        Task<Review?> CreateReviewAsync(Review reviewModel);
        Task<bool> CheckExistsAsync(string writerUserId, int reservationId);
        Task<List<Review>> GetReviewsForWriterAsync(string writerUserId);
        Task<List<Review>> GetWorkersReviewsAsync(string userId);
    }
}
