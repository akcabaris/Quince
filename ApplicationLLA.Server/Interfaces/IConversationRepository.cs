using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IConversationRepository
    {
        Task<List<Conversation>> GetConversationsAsync(string userId);
        Task<bool> CheckExistsByIdAsync(int chatId);
        Task<bool> CheckExistByUsersAsync(string userIdFirst);
        Task<bool> CheckExistByUsersAsync(string userIdFirst, string userIdSecond);
        Task<Conversation> CreateAsync(Conversation conversationModel);
        Task<Conversation?> GetConversationByIdAsync(int id, string userId);

        Task<Conversation?> GetByIdForCreateDelete(int id);

        Task<Conversation?> GetBackTheConversationAsync(string firstUserId, string secondUserID);

        Task<Conversation?> DeleteAsync(int conversationId, string userId);
        Task<int> NumberOfUnreadMessage(DateTime lastReadDate, int conversationId, string userId);
    }
}
