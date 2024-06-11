using ApplicationLLA.Server.Extensions;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.AspNetCore.Authorization;
using ApplicationLLA.Server.Mappers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ApplicationLLA.Server.Dtos.Worker;
using ApplicationLLA.Server.Service;
using ApplicationLLA.Server.Dtos.Review;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkerController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWorkerRepository _workerRepo;
        private readonly IFileService _fileService;
        private readonly IReviewRepository _reviewRepo;

        public WorkerController(UserManager<AppUser> userManager, IWorkerRepository workerRepo, IFileService fileService, IReviewRepository reviewRepo)
        {
            _userManager = userManager;
            _workerRepo = workerRepo;
            _fileService = fileService;
            _reviewRepo = reviewRepo;
        }


        [HttpGet]
        [Authorize(Roles = "Worker")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetAccount()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return BadRequest();

            var worker = await _workerRepo.GetByUserIdAsync(appUser.Id);

            if (worker == null) { return BadRequest(); }

            var workerDto = worker.ToWorkerDto();

            return Ok(workerDto);
        }

        [HttpPut]
        [Authorize(Roles = "Worker")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateWorkerDto workerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return BadRequest();

            var worker = await _workerRepo.GetByUserIdAsync(appUser.Id);

            if (worker == null) return BadRequest();
            workerDto.ToWorkerFromUpdate(worker.AppUserId.ToString());

            if (workerDto == null) { return BadRequest(); }
            var workerModel = await _workerRepo.UpdateAsync(worker.AppUserId, workerDto);

            if (workerModel == null) return BadRequest();
            return Ok();
        }

        [HttpPut("pic")]
        [Authorize(Roles = "Worker")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePicture([FromForm] UpdateWorkerPicDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return BadRequest();

            var worker = await _workerRepo.GetByUserIdAsync(appUser.Id);
            if (worker == null) return BadRequest();

            if (updateDto == null) { return BadRequest(); }

            string oldImage = null;
            string createdImageName = null;
            if (worker.PictureLink != null) { oldImage = worker.PictureLink; }

            if (updateDto.ImageFile != null)
            {
                if (updateDto.ImageFile?.Length > 1 * 1024 * 1024)
                {
                    return BadRequest("File size should not exceed 1 MB");
                }
                string[] allowedFileExtentions = [".jpg", ".jpeg", ".png"];
                createdImageName = await _fileService.SaveFileAsync(updateDto.ImageFile, allowedFileExtentions);
            }

            if (createdImageName == null) { return BadRequest(); }
            var workerModel = await _workerRepo.UpdateProfilePicAsync(worker.AppUserId, createdImageName);
            if (workerModel == null) return BadRequest();

            if (updateDto.ImageFile != null && oldImage != null)
                _fileService.DeleteFile(oldImage);

            return Ok();

        }

        [HttpDelete("DeletePic")]
        [Authorize(Roles = "Worker")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteProfilePicture()
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return BadRequest();

            var worker = await _workerRepo.GetByUserIdAsync(appUser.Id);
            if (worker == null || worker.PictureLink == null) return BadRequest();

            _fileService.DeleteFile(worker.PictureLink);
            var delete = await _workerRepo.DeleteProfilePicAsync(appUser.Id);


            //DeleteProfilePicAsync gonna return bool value(deleted true, not deleted false)
            if (delete) { return NoContent(); }
            return BadRequest();

        }



        [HttpGet("GetPublic")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetWorkerPublic(string workerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (workerId == null) return BadRequest();

            var worker = await _workerRepo.GetByUserIdAsync(workerId);

            if (worker == null) { return BadRequest(); }

            var score = await _reviewRepo.GetWorkersReviewScore(workerId);

            var reviews = await _reviewRepo.GetWorkersReviewsAsync(workerId);

            List<ReviewDto> reviewDtoList = new List<ReviewDto>();

            if (reviews != null)
            {
                foreach (var review in reviews)
                {
                    reviewDtoList.Add(review.ToReviewDto());
                }
            }

            return Ok(worker.ToPublicWorkerDto(reviewDtoList, score));

        }

    }
}
