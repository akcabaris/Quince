using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationLLA.Server.Models
{
    public class Worker
    {
        [Key]
        public required string AppUserId { get; set; }

        [StringLength(100)]
        public required string FullName { get; set; }
        [StringLength(254)]
        public string? PictureLink { get; set; }
        [StringLength(15)]
        public required Int64 PhoneNumber { get; set; }
        [StringLength(2000)]
        public string? Description { get; set; }
        public DateTime? BirthDate { get; set; }
        [StringLength(50)]
        public string? Occupation { get; set; }
        public int postLimit { get; set; } = 4;

        public  AppUser? AppUser { get; set; }
        public virtual List<Post>? Posts { get; set; } = new List<Post>();
        public virtual List<Message>? Messages { get; set; } = new List<Message>();
    }
}
