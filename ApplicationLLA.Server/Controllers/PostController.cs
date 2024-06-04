using ApplicationLLA.Server.Dtos.Post;
using ApplicationLLA.Server.Dtos.Reservation;
using ApplicationLLA.Server.Extensions;
using ApplicationLLA.Server.Helper;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Mappers;
using ApplicationLLA.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class PostController : ControllerBase
    {
        private readonly IPostRepository _postRepo;
        private readonly IWorkerRepository _workerRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICustomerRepository _customerRepo;
        private readonly ICategoryRepository _categoryRepository;
        public PostController(IPostRepository postRepo, IWorkerRepository workerRepo, UserManager<AppUser> userManager, ICustomerRepository customerRepo, ICategoryRepository categoryRepository)
        {
            _workerRepo = workerRepo;
            _customerRepo = customerRepo;
            _postRepo = postRepo;
            _userManager = userManager;
            _categoryRepository = categoryRepository;
        }

        [HttpGet("getPost")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPost([FromQuery] PostQueryObject postQueryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);



            var postModel = await _postRepo.GetPostByCityCountyCategoryAsync(postQueryObject);

            if (postQueryObject.Category != null)
            {
                await _categoryRepository.IncrementCountOfSearch(postQueryObject.Category);

            }
            var allPostDtoList = new List<AllPostDto>();



            foreach (var post in postModel)
            {
                var worker = await _workerRepo.GetByUserIdAsync(post.WorkerId);
                if (worker != null)
                {
                    var allPostDto = new AllPostDto
                    {
                        WorkerName = worker.FullName,
                        PostId = post.PostId,
                        Title = post.Title,
                        Price = post.Price,
                        PriceCurrency = post.PriceCurrency,
                        PriceWorkUnit = post.PriceWorkUnit,
                        Category = post.Category,
                        City = post.City,
                        County = post.County,
                        pictureLink = "http://localhost:5279/resources/" + worker.PictureLink,
                        Description = post.Description,
                        IsPostActive = post.IsPostActive,
                        WorkerId = post.WorkerId,

                    };
                    allPostDtoList.Add(allPostDto);
                }
            }
            if (allPostDtoList.Count > 0) { return Ok(allPostDtoList); }
            if (allPostDtoList.Count == 0) { return Ok("There aren't any posts at this address"); }
            else { return BadRequest("something went wrong"); }
        }

        [HttpGet("getPostLength")]
        public async Task<IActionResult> GetPostLength([FromQuery] PostQueryObject postQueryObject)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var postLength = await _postRepo.GetPostLenghtForSearchAsync(postQueryObject);

            return Ok(postLength);
        }

        [HttpGet("GetPostById")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetPostById()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMail = User.GetUserMail();
            if (userMail == null) return StatusCode(StatusCodes.Status500InternalServerError);


            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null)
                return BadRequest("User does not exists");


            return Ok();
        }


        [HttpGet("GetUserPosts")]
        [Authorize(Roles = "Worker")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetUserPosts()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userMail = User.GetUserMail();
            if (userMail == null) return StatusCode(StatusCodes.Status500InternalServerError);


            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null)
                return BadRequest("User does not exists");

            var worker = await _workerRepo.GetByUserIdAsync(appUser.Id.ToString());
            if (worker == null)
                return BadRequest("Worker Does not exists");

            var userPosts = await _postRepo.GetUserPostsAsync(worker);
            if (userPosts == null)
                return Ok("You don't have any Post");

            var UserPostDto = new List<UsersPostsDto>();

            foreach (var post in userPosts)
            {

                int countOfWaitingReservations = 0;
                var reservListDto = new List<ReservationDto>();

                if (post.Reservations != null)
                {
                    foreach (var reservModel in post.Reservations)
                    {

                        var customer = await _customerRepo.GetByUserIdAsync(reservModel.CustomerId);

                        if (customer != null)
                        {
                            var reservDto = reservModel.ToReservationDto(post.Title, customer.FullName, customer.PictureLink);
                            if (reservDto != null)
                            {
                                reservListDto.Add(reservDto);

                                if (reservDto.Status == "Waiting")
                                {
                                    countOfWaitingReservations++;
                                }
                            }

                        }
                    }
                }
                var postDto = post.ToUsersPostsDto(countOfWaitingReservations);

                if (postDto != null)
                {
                    postDto.ReservList = reservListDto;
                    UserPostDto.Add(postDto);
                }
            }


            return Ok(UserPostDto);
        }

        [HttpPost]
        [Authorize(Roles = "Worker")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Create([FromBody] CreatePostDto postDto)
        {
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null)
            {
                return BadRequest("User does not exists");
            }

            var worker = await _workerRepo.GetByUserIdAsync(appUser.Id.ToString());

            if (worker == null)
            {
                return BadRequest("Worker does not exists");
            }

            int postLimit = worker.postLimit;
            var checkLimit = await _postRepo.CheckLimit(worker.AppUserId.ToString(), postLimit);

            if (checkLimit)
            {
                var postModel = postDto.ToPostFromCreate(worker.AppUserId);
                await _postRepo.CreateAsync(postModel);
                return Created();
            }
            else if (checkLimit == false)
            {
                return Ok("You can create only " + postLimit + " Post.");
            }
            return BadRequest();

        }

        [HttpPut]
        [Authorize(Roles = "Worker")]
        [Route("{postId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Update([FromRoute] int postId, [FromBody] UpdatePostDto updateDto)
        {
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null) return BadRequest("User does not exists");

            var worker = await _workerRepo.GetByUserIdAsync(appUser.Id.ToString());

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (updateDto == null) return BadRequest("Update Object is null");

            var post = await _postRepo.UpdateAsync(postId, updateDto.ToPostFromUpdate(worker.AppUserId.ToString(), postId));

            if (post == null) return BadRequest("Something went wrong");

            return Ok(post.ToPostDtoOne());
        }

        [HttpDelete]
        [Authorize(Roles = "Worker")]
        [Route("{id:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var postModel = await _postRepo.DeleteAsync(id);

            if (postModel == null) return NotFound("Post does not exists");

            return Ok(postModel.ToPostDtoFromDelete());
        }
    }
}
