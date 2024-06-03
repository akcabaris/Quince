using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationLLA.Server.Models
{
    public class Conversation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [StringLength(128)]
        public required string FirstUserId { get; set; }

        [StringLength(128)]
        public required string SecondUserId { get; set; }

        public DateTime LastReadDateFirstUser { get; set; } = DateTime.MinValue;

        public DateTime LastReadDateSecondUser { get; set; } = DateTime.MinValue;
        public int NumberOfUnreadMessages { get; set; }

        public bool IsDeletedFromFirstUser { get; set; } = false;

        public bool IsDeletedFromSecondUser { get; set; } = false;

        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
