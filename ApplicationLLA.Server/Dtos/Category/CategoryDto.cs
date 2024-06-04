using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Category
{
    public class CategoryDto
    {
        [MaxLength(64)]
        public required string CategoryName { get; set; }
        public Int64 CountOfSearch { get; set; } = 0;
        public required string PictureLink { get; set; }
        public int CountOfPost {  get; set; }
    }
}
