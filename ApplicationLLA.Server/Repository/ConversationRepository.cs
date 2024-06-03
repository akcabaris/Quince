using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace ApplicationLLA.Server.Repository
{
    public class ConversationRepository : IConversationRepository
    {
        private readonly ApplicationDBContext _context;

        public ConversationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<bool> CheckExistByUsersAsync(string userIdFirst, string userIdSecond)
        {
            return await _context.Conversations.AnyAsync(x => (x.FirstUserId == userIdFirst && x.SecondUserId == userIdSecond) || (x.FirstUserId == userIdSecond && x.SecondUserId == userIdFirst));
        }

        public Task<bool> CheckExistByUsersAsync(string userIdFirst)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CheckExistsByIdAsync(int chatId)
        {
            return await _context.Conversations.AnyAsync(c => c.Id == chatId);
        }

        public async Task<Conversation> CreateAsync(Conversation conversationModel)
        {
            await _context.Conversations.AddAsync(conversationModel);
            await _context.SaveChangesAsync();
            return conversationModel;
        }
        public async Task<Conversation?> DeleteAsync(int conversationId, string userId)
        {
            var conversationModel = await _context.Conversations.FirstOrDefaultAsync(c => c.Id == conversationId);

            if (conversationModel == null) return null;

            if (conversationModel.FirstUserId == userId)
            {
                conversationModel.IsDeletedFromFirstUser = true;
            }
            else if(conversationModel.SecondUserId == userId)
            {
                conversationModel.IsDeletedFromSecondUser= true;
            }
            else{ return null; }

            if(conversationModel.IsDeletedFromFirstUser && conversationModel.IsDeletedFromSecondUser)
            {
                _context.Conversations.Remove(conversationModel);
            }

            await _context.SaveChangesAsync();

            return conversationModel;

        }

        public async Task<Conversation?> GetBackTheConversationAsync(string firstUserId, string secondUserID)
        {
            var conversationModel = await _context.Conversations.FirstOrDefaultAsync(x => (x.FirstUserId == firstUserId && x.SecondUserId == secondUserID) || (x.FirstUserId== secondUserID && x.SecondUserId == firstUserId));
            
            if(conversationModel == null)
            {
                return null;
            }

            if (conversationModel.FirstUserId == firstUserId)
            {
                conversationModel.IsDeletedFromFirstUser = false;
            }else
            {
                conversationModel.IsDeletedFromSecondUser = false;
            }
            await _context.SaveChangesAsync();
            return conversationModel;
        }

        public async Task<Conversation?> GetConversationByIdAsync(int id, string userId)
        {
            var conversationModel = await _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefaultAsync(c => c.Id == id);

            if(conversationModel == null) { return null; }

            if (conversationModel.FirstUserId == userId)
            {
                conversationModel.LastReadDateFirstUser = DateTime.Now;
            }
            else if (conversationModel.SecondUserId == userId)
            {
                conversationModel.LastReadDateSecondUser = DateTime.Now;
            }
            else
            {
                return null;
            }

            await _context.SaveChangesAsync();

            if (conversationModel.Messages != null)
            {
                conversationModel.Messages = conversationModel.Messages
                    .Where(m => (m.SenderId == userId && m.IsDeletedFromSender == false) ||
                                (m.ReceiverId == userId && m.IsDeletedFromReceiver == false))
                    .ToList();
            }



            return conversationModel;
        }

        public async Task<List<Conversation>> GetConversationsAsync(string userId)
        {

            var conversationList = await _context.Conversations
                .Where(c =>
                    ((c.FirstUserId == userId && c.IsDeletedFromFirstUser == false) ||
                    (c.SecondUserId == userId && c.IsDeletedFromSecondUser == false))
                )
                .ToListAsync();

            return conversationList;
        }

        public async Task<Conversation?> GetByIdForCreateDelete(int id)
        {
            var conversationModel = await _context.Conversations.FirstOrDefaultAsync(c => c.Id == id);

            return conversationModel;
        }


        public async Task<int> NumberOfUnreadMessage(DateTime lastReadDate, int conversationId, string userId)
        {
            var conversation = await _context.Conversations.Include(c => c.Messages).FirstOrDefaultAsync(c => c.Id == conversationId);


            if (conversation == null)
            {
                return 0;
            }

            int unreadMessages = conversation.Messages.Count(message => message.ReceiverId == userId && message.SentDate > lastReadDate);

            return unreadMessages;
        }
    }
}
