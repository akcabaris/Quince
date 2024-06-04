using ApplicationLLA.Server.Dtos.Post;
using ApplicationLLA.Server.Helper;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IPostRepository
    {
        Task<List<Post>> GetPostByCityCountyCategoryAsync(PostQueryObject postQueryObject);
        Task<int> GetPostLenghtForSearchAsync(PostQueryObject postQueryObject);
        Task<List<Post>> GetUserPostsAsync (Worker worker);
        Task<Post?> GetPostByIdAsync(int id);
        Task<Post> CreateAsync(Post postModel);
        Task<bool> CheckLimit(string workerId, int limit);
        Task<Post?> UpdateAsync(int id, Post postModel);
        Task<Post?> DeleteAsync(int id);
        Task<int> GetCountPostOfCategory(string category);
    }
}
