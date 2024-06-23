using ApplicationLLA.Server.Dtos.Message;
using ApplicationLLA.Server.Extensions;
using ApplicationLLA.Server.Helper;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Mappers;
using ApplicationLLA.Server.Models;
using ApplicationLLA.Server.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MessageController : ControllerBase
    {

        private readonly IMessageRepository _messageRepository;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConversationRepository _conversationRepo;


        public MessageController(IMessageRepository messageRepository, UserManager<AppUser> userManager, IConversationRepository conversationRepo)
        {
            _messageRepository = messageRepository;
            _userManager = userManager;
            _conversationRepo = conversationRepo;
        }

        [HttpPost]
        [Authorize(Roles ="Customer, Worker")]
        [Route("{conversationId:int}")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Create([FromRoute] int conversationId, [FromBody] CreateMessageDto createMessageDto)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var userMail = User.GetUserMail();
            if (userMail == null) return BadRequest();

            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null) return BadRequest();

            var conversationModel = await _conversationRepo.GetByIdForCreateDelete(conversationId);


            if (conversationModel == null)
                return BadRequest();

            // If user is not included in the conversation he can't send message
            if(conversationModel.FirstUserId.ToString() != appUser.Id.ToString() && conversationModel.SecondUserId.ToString() != appUser.Id.ToString())
                return BadRequest("No");

            // if user deleted the conversiton he can't send message there
            if(conversationModel.FirstUserId.ToString() == appUser.Id.ToString() && conversationModel.IsDeletedFromFirstUser == true)
            {
                return BadRequest("You cannot send message to deleted conversation");
            }
            else if (conversationModel.SecondUserId.ToString() == appUser.Id.ToString() && conversationModel.IsDeletedFromSecondUser == true)
            {
                return BadRequest("You cannot send message to deleted conversation");
            }

            string receiverId;

            if (conversationModel.FirstUserId == appUser.Id.ToString())
                { receiverId = conversationModel.SecondUserId.ToString(); }
            else { receiverId = conversationModel.FirstUserId.ToString(); }

            createMessageDto.Content = createMessageDto.Content.HandleSpaces();

            var messageModel = createMessageDto.ToMessageFromCreateDto(conversationModel,appUser.Id.ToString(),receiverId);

            if (messageModel == null) return BadRequest();

            var result = await _messageRepository.CreateAsync(messageModel);

            if(result == null) return BadRequest();

            return Created();
        }

        [HttpPut]
        [Authorize(Roles = "Customer, Worker")]
        [Route("{conversationId:int}")]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromRoute] int conversationId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var userMail = User.GetUserMail();
            if (userMail == null) return BadRequest();

            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null) return BadRequest();

            var conversationModel = await _conversationRepo.GetByIdForCreateDelete(conversationId);


            if (conversationModel == null)
                return BadRequest();

            // If user is not included in the conversation he can't delete messages
            if (conversationModel.FirstUserId.ToString() != appUser.Id.ToString() && conversationModel.SecondUserId.ToString() != appUser.Id.ToString())
                return BadRequest("You can't delete other user's messages");


            var message = await _messageRepository.DeleteAsync(conversationId,appUser.Id.ToString());

            if (!message) return NotFound();

            return Ok();
        }
    }
}
