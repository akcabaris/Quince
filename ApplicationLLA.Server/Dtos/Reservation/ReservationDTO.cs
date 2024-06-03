using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Reservation
{
    public class ReservationDto
    {
        public int ReservationId { get; set; }
        public required string CustomerId { get; set; }
        public int PostId { get; set; }
        public required string Status { get; set; }
        public required DateTime ReservationDate { get; set; }
        [MaxLength(200)]
        public required string ReservationNote { get; set; }
        [MaxLength(240)]
        public required string CustomerPictureLink { get; set; }
        public required string PostTitle { get; set; }
        public required string CustomerName { get; set; }
    }
}
