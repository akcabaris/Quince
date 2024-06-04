using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface ICategoryRepository
    {
        Task<Category[]> GetCategories();

        Task<bool> IncrementCountOfSearch(string categoryName);
    }
}
