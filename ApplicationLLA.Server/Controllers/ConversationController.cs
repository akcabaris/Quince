using ApplicationLLA.Server.Dtos.Conversation;
using ApplicationLLA.Server.Dtos.Message;
using ApplicationLLA.Server.Extensions;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Mappers;
using ApplicationLLA.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ApplicationLLA.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IConversationRepository _conversationRepo;
        private readonly IMessageRepository _messageRepo;
        private readonly IWorkerRepository _workerRepo;
        private readonly ICustomerRepository _customerRepo;

        public ConversationController(UserManager<AppUser> userManager, IConversationRepository conversationRepo, IMessageRepository messageRepo, IWorkerRepository workerRepo, ICustomerRepository customerRepo)
        {
            _userManager = userManager;
            _conversationRepo = conversationRepo;
            _messageRepo = messageRepo;
            _workerRepo = workerRepo;
            _customerRepo = customerRepo;

        }

        [HttpGet]
        [Authorize(Roles =("Customer, Worker"))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetConversations()
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var userMail = User.GetUserMail();
            if (userMail == null) { return BadRequest(); }

            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null) { return BadRequest(); }

            var conversations = await _conversationRepo.GetConversationsAsync(appUser.Id.ToString());

            if (conversations==null) { return BadRequest(); }

            if (conversations.Count() == 0) return Ok();

            var conversationDtoList = new List<ConversationDto>();


            foreach (var conversation in conversations
                 )
            { 
                string userFullname = "Application User";
                string pictureLink = "empty";
                var convDto = new ConversationDto();

                var userId = conversation.FirstUserId == appUser.Id.ToString() ? conversation.SecondUserId : conversation.FirstUserId;

                var lastReadDate = conversation.FirstUserId == appUser.Id.ToString() ? conversation.LastReadDateFirstUser : conversation.LastReadDateSecondUser;
      

                    if (await _workerRepo.IsWorkerExistsAsync(userId))
                    {
                        var userWorker = (await _workerRepo.GetByUserIdAsync(userId));
                        if (userWorker != null)
                        {
                            userFullname = userWorker.FullName.ToString();
                            userId = userWorker.AppUserId.ToString();
                        if(userWorker.PictureLink != null) { pictureLink = userWorker.PictureLink.ToString(); }
                            
                        }
                    }
                    else
                    {
                        var userCustomer = await _customerRepo.GetByUserIdAsync(userId);
                        if (userCustomer != null)
                        {
                            userFullname = userCustomer.FullName.ToString();
                            userId = userCustomer.AppUserId.ToString();
                        if (userCustomer.PictureLink != null) { pictureLink = userCustomer.PictureLink.ToString(); }
                        }

                    }

                var numberOfUnreadMessages = await _conversationRepo.NumberOfUnreadMessage(lastReadDate, conversation.Id, appUser.Id.ToString());
               
                if (userFullname != null)
                    { convDto = conversation.ToConversationDto(userFullname, pictureLink, userId, lastReadDate, numberOfUnreadMessages); }
                    conversationDtoList.Add(convDto);
            }

            return Ok(conversationDtoList);
        }


        // it's getting Updated Messages
        [HttpGet("Messages")]
        [Authorize(Roles = ("Customer, Worker"))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> GetConversationUpdated(int id)
        {
            if (!ModelState.IsValid) { return BadRequest(ModelState); }

            var userMail = User.GetUserMail();
            if (userMail == null) { return BadRequest(); }

            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null) { return BadRequest(); }

            var conversation = await _conversationRepo.GetConversationByIdAsync(id, appUser.Id.ToString());
            if (conversation == null) { return BadRequest(); }


            var messageDtoList = new List<MessageDto>();
            foreach (var message in conversation.Messages)
            {
                var messageDto = message.ToMessageDto();

                if (messageDto != null) { messageDtoList.Add(messageDto); }
            }

            
                return Ok(messageDtoList);
        }

        


        [HttpPost]
        [Authorize]
        [Route("{userId}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Create([FromRoute] string userId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!(await _workerRepo.IsWorkerExistsAsync(userId)) && !(await _customerRepo.IsCustomerExistsAsync(userId)))
            {
                return NotFound("User does not exists");
            }

            var userMail = User.GetUserMail();
            if (userMail == null) { return BadRequest(); }

            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null) { return BadRequest(); }

            if (userId == appUser.Id.ToString()) { return BadRequest("You can not talk with yourself"); }

            var createConversationDto = new CreateConversationDto();
            int conversationId;

            if (await _conversationRepo.CheckExistByUsersAsync(appUser.Id.ToString(), userId))
            {
                var conversationBack = await _conversationRepo.GetBackTheConversationAsync(appUser.Id.ToString(), userId);
                
                if (conversationBack != null)
                {
                    conversationId = conversationBack.Id;
                    return Ok(conversationId); 
                }
            }

            createConversationDto.FirstUserId = appUser.Id.ToString();
            createConversationDto.SecondUserId = userId;

            var result = await _conversationRepo.CreateAsync(createConversationDto.ToConversationFromCreate());

            if (result == null) return BadRequest();

            conversationId = result.Id;
            return Ok(conversationId);
        }

        [HttpPut]
        [Route("{conversationId:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = ("Customer, Worker"))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteAsync([FromRoute] int conversationId)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!await _conversationRepo.CheckExistsByIdAsync(conversationId))
            {
                return NotFound();
            }

            var userMail = User.GetUserMail();
            if (userMail == null) { return BadRequest(); }

            var appUser = await _userManager.FindByEmailAsync(userMail);
            if (appUser == null) { return BadRequest(); }


            bool isDeletedMessages = await _messageRepo.DeleteAsync(conversationId, appUser.Id.ToString());
            if (isDeletedMessages == false) return BadRequest("Messages couldn't delete");

            var isConversationDeleted = await _conversationRepo.DeleteAsync(conversationId, appUser.Id.ToString());


            if (isConversationDeleted != null)
            {
                return Ok("it's deleted");
            }
            else
            {
                return BadRequest("Conversation couldn't delete");
            }
        }
    }
}
