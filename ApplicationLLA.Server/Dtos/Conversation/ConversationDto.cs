using ApplicationLLA.Server.Dtos.Message;

namespace ApplicationLLA.Server.Dtos.Conversation
{
    public class ConversationDto
    {
        public int conversationId { get; set; }
        public string userId { get; set; } = string.Empty;
        public string userFullName { get; set; } = string.Empty;
        public DateTime? LastReadDate { get; set; }
        public int NumberOfUnreadMessages { get; set; }
        public string? pictureLink { get; set; }

    }
}
