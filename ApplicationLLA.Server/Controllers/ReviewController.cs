using ApplicationLLA.Server.Dtos.Review;
using ApplicationLLA.Server.Extensions;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Mappers;
using ApplicationLLA.Server.Models;
using ApplicationLLA.Server.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepo;
        private readonly UserManager<AppUser> _userManager;
        private readonly IWorkerRepository _workerRepo;
        private readonly ICustomerRepository _customerRepo;

        public ReviewController(IReviewRepository reviewRepo, UserManager<AppUser> userManager, IWorkerRepository workerRepo, ICustomerRepository customerRepo)
        {
            _userManager = userManager;
            _reviewRepo = reviewRepo;
            _customerRepo = customerRepo;
            _workerRepo = workerRepo;
        }


        [HttpGet("GetReviews")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviews(string workerId)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            if (workerId == null) { return BadRequest(); }

            if (await _workerRepo.IsWorkerExistsAsync(workerId)) { return BadRequest(); }

            var reviews = await _reviewRepo.GetWorkersReviewsAsync(workerId);

            List<ReviewDto> reviewDtoList = new List<ReviewDto>();

            if (reviews != null)
            {
                foreach (var review in reviews)
                {
                    reviewDtoList.Add(review.ToReviewDto());
                }
            }
            return Ok(reviewDtoList);

        }

        [HttpGet("GetReviewsForProfile")]
        [Authorize("Worker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetReviewsForProfile()
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return Unauthorized();

            List<Review> reviews = new List<Review>();

            if (await _workerRepo.IsWorkerExistsAsync(appUser.Id.ToString()))
            {
                reviews = await _reviewRepo.GetWorkersReviewsAsync(appUser.Id.ToString());
            }
            else if (await _customerRepo.IsCustomerExistsAsync(appUser.Id.ToString()))
            {
                reviews = await _reviewRepo.GetReviewsForWriterAsync(appUser.Id.ToString());
            }
            else
            {
                return BadRequest();
            }

            List<ReviewDto> reviewDtoList = new List<ReviewDto>();

            if (reviews != null && reviews.Count > 0)
            {
                foreach (var review in reviews)
                {
                    reviewDtoList.Add(review.ToReviewDto());
                }
            }

            return Ok(reviewDtoList);
        }

        [HttpPost]
        [Authorize("Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return Unauthorized();

            if (!(await _customerRepo.IsCustomerExistsAsync(appUser.Id.ToString())))
            {
                return BadRequest();
            }

            if((await _reviewRepo.CheckExistsAsync(appUser.Id.ToString(), createReviewDto.ReservationId)))
            {
                return Ok("Review Already Exists");
            }

            var createReview = await _reviewRepo.CreateReviewAsync(createReviewDto.ToReviewFromCreateDto(appUser.Id.ToString()));

            if (createReview == null) { return StatusCode(500); }

            return Ok(createReview.ToReviewDto());
        }

    }
}
