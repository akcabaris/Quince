using System.ComponentModel.DataAnnotations;

namespace ApplicationLLA.Server.Dtos.Reservation
{
    public class CreateReservationDto
    {
        public required DateTime ReservationDate { get; set; }
        [MaxLength(200)]
        public required string ReservationNote { get; set; } = string.Empty;
    }
}
