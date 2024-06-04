using ApplicationLLA.Server.Dtos.Category;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Mappers;
using ApplicationLLA.Server.Models;
using ApplicationLLA.Server.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IPostRepository _postRepository;


        public CategoryController(ICategoryRepository categoryRepository, IPostRepository postRepository)
        {
            _categoryRepository = categoryRepository;
            _postRepository = postRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetCategories()
        {
            var categoryList = await _categoryRepository.GetCategories();

            var categoryDtoList = new List<CategoryDto>();

            foreach (var category in categoryList)
            {
                var countOfPost = await _postRepository.GetCountPostOfCategory(category.CategoryName);
                var categoryDto = category.ToCategoryDto(countOfPost);
                categoryDtoList.Add(categoryDto);
            }

            return Ok(categoryDtoList);
        }
    }
}
