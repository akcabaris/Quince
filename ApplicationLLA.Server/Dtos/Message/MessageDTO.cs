using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Message
{
    public class MessageDto
    {
        public int MessageId { get; set; }

        public string SenderId { get; set; }

        [StringLength(300, MinimumLength =1)]
        public required string Content { get; set; }
        public required DateTime SentDate { get; set; }
    }
}
