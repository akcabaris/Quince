using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApplicationLLA.Server.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        public required string AppUserId { get; set; }

        [StringLength(254)]
        public string? PictureLink { get; set; }

        [StringLength(40)]
        public required string FullName { get; set; }

        [StringLength(15)]
        public required Int64 PhoneNumber { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(16)]
        public string? Gender { get; set; }
        public AppUser? AppUser { get; set; }
        public List<Reservation> Reservations { get; set; } = new List<Reservation>();
        public List<Message> Messages { get; set; } = new List<Message>();
    }
}
