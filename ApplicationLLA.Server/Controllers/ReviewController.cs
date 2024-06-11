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
        private readonly IReservationRepository _reservationRepo;
        private readonly IPostRepository _postRepo;

        public ReviewController(IReviewRepository reviewRepo, UserManager<AppUser> userManager, IWorkerRepository workerRepo, ICustomerRepository customerRepo, IReservationRepository reservationRepo, IPostRepository postRepo)
        {
            _userManager = userManager;
            _reviewRepo = reviewRepo;
            _customerRepo = customerRepo;
            _workerRepo = workerRepo;
            _reservationRepo = reservationRepo;
            _postRepo = postRepo;
        }


        [HttpGet("GetReviews")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviews(string workerId)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            if (workerId == null) { return BadRequest(); }

            if (!(await _workerRepo.IsWorkerExistsAsync(workerId))) { return BadRequest("Worker does not exists"); }

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


        [HttpPost]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateReview([FromBody] CreateReviewDto createReviewDto)
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return Unauthorized();

            if (!(await _customerRepo.IsCustomerExistsAsync(appUser.Id.ToString())))
            {
                return Unauthorized();
            }

            // check the owner of the reservation
            if (!(await _reservationRepo.CheckIsOwnerRight(appUser.Id, createReviewDto.ReservationId)))
            {
                return StatusCode(403);
            }

            if ((await _reviewRepo.CheckExistsAsync(appUser.Id.ToString(), createReviewDto.ReservationId)))
            {
                return Ok("Review Already Exists");
            }

            var postId = await _reservationRepo.GetPostIdFromReservation(createReviewDto.ReservationId);

            var workerId = await _postRepo.GetWorkerIdFromPost(postId);
            if(workerId == null) { return StatusCode(500); }

            var reservationStatus = await _reservationRepo.GetReservationStatusById(createReviewDto.ReservationId);

            if (reservationStatus == "Done")
            {
                var createReview = await _reviewRepo.CreateReviewAsync(createReviewDto.ToReviewFromCreateDto(appUser.Id, workerId));

                if (createReview == null) { return StatusCode(500); }

                return Ok(createReview.ToReviewDto());
            }
            else
            {
                return Ok("You can Write a Review only if the reservation status is Done");
            }
        }


        [HttpGet("GetReviewsForProfile")]
        [Authorize(Roles = "Worker")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetReviewsForProfile()
        {
            if (!ModelState.IsValid) { return BadRequest(); }

            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return Unauthorized();

            if (!(await _workerRepo.IsWorkerExistsAsync(appUser.Id))) { return BadRequest("Worker does not exists"); }

            var reviews = await _reviewRepo.GetWorkersReviewsAsync(appUser.Id);

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

    }
}
