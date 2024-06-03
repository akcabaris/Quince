using ApplicationLLA.Server.Dtos.Message;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IMessageRepository
    {
        Task<Message> CreateAsync(Message messageModel);
        Task<bool> DeleteAsync(int conversationId, string userId);

    }
}
