using ApplicationLLA.Server.Dtos.Conversation;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Mappers
{
    public static class ConversationMapper
    {
        public static Conversation ToConversationFromCreate(this CreateConversationDto createConversationDto)
        {
            return new Conversation
            {
                FirstUserId = createConversationDto.FirstUserId,
                SecondUserId = createConversationDto.SecondUserId,
                IsDeletedFromFirstUser = false,
                IsDeletedFromSecondUser = false
            };
        }
        public static ConversationDto ToConversationDto(this Conversation conversation, string userFullname, string pictureLink, string userId, DateTime lastReadDate, int numberOfUnreadMessages)
        {
            return new ConversationDto
            {

                userFullName = userFullname,
                conversationId = conversation.Id,
                LastReadDate = lastReadDate,
                NumberOfUnreadMessages = numberOfUnreadMessages,
                pictureLink = "http://localhost:5279/resources/" + pictureLink,
                userId = userId
                
            };

        }
    }
}
