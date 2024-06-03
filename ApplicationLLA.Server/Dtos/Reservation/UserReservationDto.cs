using ApplicationLLA.Server.Dtos.Post;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Reservation
{
    public class UserReservationDto
    {
        public int ReservationId { get; set; }
        public required string CustomerId { get; set; }
        public required string Status { get; set; }
        public required DateTime ReservationDate { get; set; }
        [MaxLength(200)]
        public required string ReservationNote { get; set; }
        public required AllPostDto PostDto { get; set; }
    }
}
