using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.Repository
{
    public class ReviewRepository : IReviewRepository
    {

        private readonly ApplicationDBContext _context;
        public ReviewRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistsAsync(string writerUserId, int reservationId)
        {
            return await _context.Reviews.AnyAsync(r => r.ReviewWriterUserId == writerUserId && r.reservationId == reservationId);
        }


        public async Task<Review?> CreateReviewAsync(Review reviewModel)
        {
            if(reviewModel == null) { return null; }
            var create = await _context.Reviews.AddAsync(reviewModel);
            if(create == null) { return null; }
            await _context.SaveChangesAsync();
            return reviewModel;
        }

        public async Task<List<Review>> GetWorkersReviewsAsync(string userId)
        {
            return await _context.Reviews.Where(r => r.ReviewToUserId == userId).ToListAsync();
            
        }

        public async Task<List<Review>> GetReviewsForWriterAsync(string writerUserId)
        {
            var reviewList = await _context.Reviews.Where(r => r.ReviewWriterUserId == writerUserId).ToListAsync();
            return reviewList;
        }

        public async Task<bool> DeleteAsync(int reviewId)
        {
            var review = await _context.Reviews.FirstOrDefaultAsync(r => r.ReviewId == reviewId);
            if(review == null) { return false; }
            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();
            return true;

        }
    }
}
