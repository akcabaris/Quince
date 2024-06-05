using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category[]> GetCategoriesAsync();

        Task<bool> IncrementCountOfSearchAsync(string categoryName);
    }
}
