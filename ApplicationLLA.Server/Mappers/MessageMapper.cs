using ApplicationLLA.Server.Dtos.Message;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Mappers
{
    public static class MessageMapper
    {

        public static Message ToMessageFromCreateDto (this CreateMessageDto createMessageDto,Conversation conversationModel, string senderId, string receiverId)
        {

            return new Message
            {
                Content = createMessageDto.Content,
                Conversation = conversationModel,
                IsDeletedFromReceiver = false,
                SenderId = senderId,
                ReceiverId = receiverId,
                IsDeletedFromSender = false,
                ConversationId = conversationModel.Id,
                SentDate = DateTime.Now,
            };
        }

        public static MessageDto ToMessageDto (this Message messageModel)
        {
            return new MessageDto
            {
                Content = messageModel.Content,
                MessageId = messageModel.MessageId,
                SenderId = messageModel.SenderId,
                SentDate = messageModel.SentDate,
            };
        }
    }
}
