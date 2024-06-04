using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationLLA.Server.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [MaxLength(64)]
        public required string CategoryName { get; set; }
        public Int64 CountOfSearch { get; set; } = 0;
        public required string PictureLink { get; set; }

    }
}
