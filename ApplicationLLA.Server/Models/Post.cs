using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Models
{
    public class Post
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PostId { get; set; }

        [StringLength(60)]
        public required string Category { get; set; }

        [StringLength(120)]
        public required string Title { get; set; }

        [StringLength(2000)]
        public required string Description { get; set; }

        public int Price { get; set; }
        [StringLength(8)]
        [Required]
        public required string PriceCurrency {  get; set; }
        [StringLength(16)]
        [Required]
        public required string PriceWorkUnit { get; set; }

        [StringLength(30)]
        public required string City { get; set; }

        [StringLength(30)]
        public required string County { get; set; }

        public required bool IsPostActive { get; set; } = false;
        public required string WorkerId { get; set; }
        public virtual Worker? Worker { get; set; }
        public virtual List<Reservation>? Reservations { get; set; } = new List<Reservation>();

    }
}
