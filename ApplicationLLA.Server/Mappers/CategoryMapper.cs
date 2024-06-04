using ApplicationLLA.Server.Dtos.Category;
using ApplicationLLA.Server.Dtos.Conversation;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Mappers
{
    public static class CategoryMapper
    {
        public static CategoryDto ToCategoryDto(this Category category, int countOfPost)
        {
            return new CategoryDto
            {
                CategoryName = category.CategoryName,
                CountOfPost = countOfPost,
                PictureLink = category.PictureLink,
                CountOfSearch = category.CountOfSearch,
            };
        }
    }
}
