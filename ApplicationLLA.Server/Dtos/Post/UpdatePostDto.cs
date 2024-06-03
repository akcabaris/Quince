using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Post
{
    public class UpdatePostDto
    {
        public required string Category { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public int Price { get; set; }
        [StringLength(8)]
        [Required]
        public required string PriceCurrency { get; set; }
        [StringLength(16)]
        [Required]
        public required string PriceWorkUnit { get; set; }
        public required string City { get; set; }
        public required string County { get; set; }
        public required bool IsPostActive { get; set; } = true;
    }
}
