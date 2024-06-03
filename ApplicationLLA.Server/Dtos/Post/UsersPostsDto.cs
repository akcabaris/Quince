using ApplicationLLA.Server.Dtos.Reservation;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Post
{
    public class UsersPostsDto
    {

        public  int PostId { get; set; }
        public string WorkerId { get; set; } = string.Empty;
        public string Category { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        [StringLength(8)]
        [Required]
        public required string PriceCurrency { get; set; }
        [StringLength(16)]
        [Required]
        public required string PriceWorkUnit { get; set; }
        public string City { get; set; }
        public string County { get; set; }
        public int CountOfWaitingReservation { get; set; }
        public bool IsPostActive { get; set; } = true;
        public List<ReservationDto>? ReservList { get; set; } = new List<ReservationDto>();
    }
}
