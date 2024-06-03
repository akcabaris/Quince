using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Dtos.Message;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.Repository
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDBContext _context;

        public MessageRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Message> CreateAsync(Message messageModel)
        {
            await _context.Messages.AddAsync(messageModel);

            var conversation = await _context.Conversations.FirstOrDefaultAsync(c => c.Id == messageModel.ConversationId);

            if (conversation != null && conversation.FirstUserId == messageModel.ReceiverId && conversation.IsDeletedFromFirstUser == true)
            {
                conversation.IsDeletedFromFirstUser = false;
            }
            if (conversation != null && conversation.SecondUserId == messageModel.ReceiverId && conversation.IsDeletedFromSecondUser == true)
            {
                conversation.IsDeletedFromSecondUser = false;
            }

            await _context.SaveChangesAsync();
            return messageModel;
        }

        public async Task<bool> DeleteAsync(int conversationId, string userId)
        {
            List<Message> messages = new List<Message>();

            messages = await _context.Messages
                .Where(m => m.ConversationId == conversationId)
                .ToListAsync();

            if (messages != null)
            {
                foreach (var message in messages)
                {
                    if (message.SenderId == userId)
                    {
                        message.IsDeletedFromSender = true;
                    }
                    else if (message.ReceiverId == userId)
                    {
                        message.IsDeletedFromReceiver = true;
                    }

                    if (message.IsDeletedFromSender && message.IsDeletedFromReceiver)
                    {
                        _context.Messages.Remove(message);
                    }
                }

            }
            await _context.SaveChangesAsync();

            if (messages != null) return true;
            else return false;


        }
    }
}
