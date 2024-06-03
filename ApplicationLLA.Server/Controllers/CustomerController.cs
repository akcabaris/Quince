using ApplicationLLA.Server.Dtos.Customer;
using ApplicationLLA.Server.Extensions;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Mappers;
using ApplicationLLA.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ICustomerRepository _customerRepo;
        private readonly IFileService _fileService;

        public CustomerController(UserManager<AppUser> userManager, ICustomerRepository customerRepo, IFileService fileService)
        {
            _userManager = userManager;
            _customerRepo = customerRepo;
            _fileService = fileService;
        }

        [HttpGet]
        [Authorize(Roles ="Customer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAccount()
        {
            if(!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return BadRequest();

            var customer = await _customerRepo.GetByUserIdAsync(appUser.Id);

            if (customer == null) { return BadRequest(); }

            var customerDto = customer.ToCustomerDto();

            return Ok(customerDto);
        }

        [HttpPut]
        [Authorize(Roles ="Customer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateAccount([FromBody] UpdateCustomerDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return BadRequest();

            var customer = await _customerRepo.GetByUserIdAsync(appUser.Id);

            if (customer == null) return BadRequest();
            customerDto.ToCustomerFromUpdate(customer.AppUserId.ToString());

            if (customerDto == null) { return BadRequest(); }
            var customerModel = await _customerRepo.UpdateAsync(customer.AppUserId, customerDto);

            if (customerModel == null) return BadRequest();
            return Ok();
        }


        [HttpPut("pic")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePicture([FromForm] UpdateCustomerPicDto updateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userMail = User.GetUserMail();
            var appUser = await _userManager.FindByEmailAsync(userMail);

            if (appUser == null) return BadRequest();

            var customer = await _customerRepo.GetByUserIdAsync(appUser.Id);
            if (customer == null) return BadRequest();

            if (updateDto == null) { return BadRequest(); }

            string oldImage = null;
            string createdImageName = null;
            if (customer.PictureLink != null) { oldImage = customer.PictureLink; }

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
            var customerModel = await _customerRepo.UpdateProfilePicAsync(customer.AppUserId, createdImageName);
            if (customerModel == null) return BadRequest();

            if (updateDto.ImageFile != null && oldImage != null)
                _fileService.DeleteFile(oldImage);

            return Ok();

        }

        [HttpDelete("DeletePic")]
        [Authorize(Roles = "Customer")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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

            var customer = await _customerRepo.GetByUserIdAsync(appUser.Id);
            if (customer == null || customer.PictureLink == null) return BadRequest();

            _fileService.DeleteFile(customer.PictureLink);
            var delete = await _customerRepo.DeleteProfilePicAsync(appUser.Id);

            //DeleteProfilePicAsync gonna return bool value(deleted true, not deleted false)
            if (delete) { return NoContent(); }
            return BadRequest();
        }
    }
}
