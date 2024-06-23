using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category[]> GetCategoriesAsync();
        Task<List<string>> GetCategoryNamesForCheck();

        Task<bool> IncrementCountOfSearchAsync(string categoryName);
    }
}
