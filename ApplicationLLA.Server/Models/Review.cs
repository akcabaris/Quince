using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }
        [MaxLength(240)]
        public required string ReviewText { get; set; }
        [Range(1,5, ErrorMessage ="Review score can be just integer 1 to 5")]
        public int ReviewScore { get; set; }
        // I'm aware this variable start with a lower case, But i won't create a migrate for just changing this.
        public int reservationId { get; set; }
        [MaxLength(64)]
        public required string ReviewToUserId { get; set; }
        [MaxLength(64)]
        public required string ReviewWriterUserId { get; set; }
    }
}
