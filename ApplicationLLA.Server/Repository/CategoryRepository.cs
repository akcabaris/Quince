using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.Repository
{
    public class CategoryRepository : ICategoryRepository
    {

        private readonly ApplicationDBContext _context;
        public CategoryRepository(ApplicationDBContext context)
        {
            _context = context;
        }
        public async Task<Category[]> GetCategoriesAsync() => await _context.Categories.ToArrayAsync();

        public async Task<List<string>> GetCategoryNamesForCheck()
        {
            return await _context.Categories.Select(c => c.CategoryName).ToListAsync();
        }


        public async Task<bool> IncrementCountOfSearchAsync(string categoryName)
        {
            var category = await _context.Categories.FirstOrDefaultAsync(c => c.CategoryName == categoryName);
            if (category == null)
            {
                return false;
            }
            category.CountOfSearch += 1;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
