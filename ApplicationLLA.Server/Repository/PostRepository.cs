using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Dtos.Post;
using ApplicationLLA.Server.Helper;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ApplicationLLA.Server.Repository
{


    public class PostRepository : IPostRepository
    {
        private readonly ApplicationDBContext _context;

        public PostRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckLimit(string workerId, int limit)
        {
            int countOfUserPosts = await _context.Posts.CountAsync(p => p.WorkerId == workerId);

            if(countOfUserPosts < limit)
            {
                return true;
            }

            return false;
        }

        public async Task<Post> CreateAsync(Post postModel)
        {

            await _context.Posts.AddAsync(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }

        public async Task<Post?> DeleteAsync(int id)
        {
            var postModel = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);

            if (postModel == null) return null;
            _context.Posts.Remove(postModel);
            await _context.SaveChangesAsync();
            return postModel;
        }

        public async Task<int> GetCountPostOfCategory(string category)
        {
            return await _context.Posts.CountAsync(x => x.Category == category);
        }

        public async Task<List<Post>> GetPostByCityCountyCategoryAsync(PostQueryObject postQueryObject)
        {
            var posts = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(postQueryObject.City))
            {
                posts = posts.Where(s => s.City == postQueryObject.City);
            }
            if (!string.IsNullOrWhiteSpace(postQueryObject.County))
            {
                posts = posts.Where(s => s.County == postQueryObject.County);
            }

            if (!string.IsNullOrWhiteSpace(postQueryObject.Category))
            {
                posts = posts.Where(s => s.Category.Contains(postQueryObject.Category));
            }
            posts = posts.Where(s => s.IsPostActive == true);

            var skipNumber = (postQueryObject.PageNumber - 1) * postQueryObject.PageSize;

            return await posts.Skip(skipNumber).Take(postQueryObject.PageSize).ToListAsync();
        }

        public async Task<Post?> GetPostByIdAsync(int id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.PostId == id);

            if (post.IsPostActive == false)
            {
                return null;
            }

            return post;
        }

        public async Task<int> GetPostLenghtForSearchAsync(PostQueryObject postQueryObject)
        {
            var posts = _context.Posts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(postQueryObject.City))
            {
                posts = posts.Where(s => s.City == postQueryObject.City);
            }
            if (!string.IsNullOrWhiteSpace(postQueryObject.County))
            {
                posts = posts.Where(s => s.County == postQueryObject.County);
            }

            if (!string.IsNullOrWhiteSpace(postQueryObject.Category))
            {
                posts = posts.Where(s => s.Category.Contains(postQueryObject.Category));
            }
            posts = posts.Where(s => s.IsPostActive == true);

            return (await posts.CountAsync());
        }

        // buna bir dto yazılacak ***************
        public async Task<List<Post>> GetUserPostsAsync(Worker worker)
        {
            var userPostList = new List<Post>();

            userPostList =  await _context.Posts
                  .Where(u => u.WorkerId == worker.AppUserId)
                  .Include(post => post.Reservations)
                    .Select(post => new Post
                    {
                        WorkerId = worker.AppUserId,
                        PostId = post.PostId,
                        City = post.City,
                        County = post.County,
                        Description = post.Description,
                        Price = post.Price,
                        PriceCurrency = post.PriceCurrency,
                        PriceWorkUnit   = post.PriceWorkUnit,
                        Title = post.Title,
                        Category = post.Category,
                        IsPostActive = post.IsPostActive,
                        Reservations = post.Reservations,
                    }).ToListAsync();



            return userPostList;
        }

        public async Task<Post?> UpdateAsync(int id, Post postModel)
        {
            var existingPost = await _context.Posts.FindAsync(id);

            if (existingPost == null) return null;

            existingPost.IsPostActive = postModel.IsPostActive;
            existingPost.Description = postModel.Description;
            existingPost.Price = postModel.Price;
            existingPost.PriceCurrency = postModel.PriceCurrency;
            existingPost.PriceWorkUnit = postModel.PriceWorkUnit;
            existingPost.Title = postModel.Title;
            existingPost.Category = postModel.Category;
            existingPost.City = postModel.City;
            existingPost.County = postModel.County;

            await _context.SaveChangesAsync();

            return existingPost;
        }

    }
}
