using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationLLA.Server.Models
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MessageId { get; set; }

        public required int ConversationId { get; set; }

        [StringLength(128)]
        public required string SenderId { get; set; }

        [StringLength(128)]
        public required string ReceiverId { get; set; }

        public bool IsDeletedFromSender { get; set; }
        public bool IsDeletedFromReceiver { get; set; }

        [StringLength(300)]
        public required string Content { get; set; }
        public required DateTime SentDate { get; set; }

        public required Conversation Conversation { get; set; }
    }

}
