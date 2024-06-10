using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.Repository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDBContext _context;
        public ReservationRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Reservation?> ApproveOrDenyReservationAsync(int id, string status)
        {
            var existingReserv = await _context.Reservations.FirstOrDefaultAsync(x => x.ReservationId == id);

            if (existingReserv == null) return null;

            if (!(status == "Approved" || status == "Denied" || status == "Done")) status = "Waiting";

            existingReserv.Status = status;

            await _context.SaveChangesAsync();

            return existingReserv;
        }

        public async Task<string> GetReservationStatus(string customerId, int postId)
        {
            var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.CustomerId == customerId && x.PostId == postId);
            if (reservation == null) { return "Reservation isn't exists"; }

            return reservation.Status;

        }

        public async Task<string> GetReservationStatusById(int reservationId)
        {

            var reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.ReservationId == reservationId);
            if (reservation == null) { return "Reservation isn't exists"; }

            return reservation.Status;

        }


        public async Task<Reservation> CreateAsync(Reservation reservModel)
        {

            await _context.Reservations.AddAsync(reservModel);
            await _context.SaveChangesAsync();
            return reservModel;
        }

        public async Task<Reservation?> DeleteAsync(int id)
        {
            var reservModel = await _context.Reservations.FirstOrDefaultAsync(x => x.ReservationId == id);

            if (reservModel == null) return null;

            _context.Remove(reservModel);
            await _context.SaveChangesAsync();
            return reservModel;
        }

        public async Task<Reservation?> GetByIdAsync(int id)
        {
            return await _context.Reservations.FirstOrDefaultAsync(x => x.ReservationId == id);
        }

        public async Task<List<Reservation>> GetUsersReservationsAsync(Customer customer)
        {
            return await _context.Reservations
                .Where(u => u.CustomerId == customer.AppUserId)
                .Select(reservation => new Reservation
                {
                    CustomerId = customer.AppUserId,
                    PostId = reservation.PostId,
                    ReservationDate = reservation.ReservationDate,
                    Status = reservation.Status,
                    ReservationNote = reservation.ReservationNote,
                    ReservationId = reservation.ReservationId

                }).ToListAsync();


        }

        public async Task<bool> CheckIsOwnerRight(string userId, int reservationId)
        {
            return await _context.Reservations.AnyAsync(r => r.ReservationId == reservationId && r.CustomerId == userId);

        }

        public async Task<bool> CheckIsReservationExists(string userId, int postId)
        {
            return await _context.Reservations.AnyAsync(r => r.CustomerId == userId && r.PostId == postId);
        }

        public async Task<int> GetPostIdFromReservation(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null) return 0;
            return reservation.PostId;
        }
    }
}
