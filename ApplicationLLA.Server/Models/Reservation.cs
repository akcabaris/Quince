using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ApplicationLLA.Server.Models
{
    public class Reservation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReservationId { get; set; }
        public required string CustomerId { get; set; }
        public required int PostId { get; set; }

        [DefaultValue("Waiting")]
        public required string Status { get; set; }
        public required DateTime ReservationDate { get; set; }
        [MaxLength(200)]
        public required string ReservationNote { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual Post Post { get; set; }
    }
}
