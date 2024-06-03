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
            var existingReserv = await _context.Reservations.FirstOrDefaultAsync(x=> x.ReservationId == id);

            if (existingReserv == null) return null;

            if (!(status == "Approved" || status == "Denied" || status == "Done")) status = "Waiting";

            existingReserv.Status = status;

            await _context.SaveChangesAsync();

            return existingReserv;
        }

        public async Task<bool> CheckReservationExistsAsync(Customer customer, Post post)
        {
            return await _context.Reservations.AnyAsync(x => x.CustomerId == customer.AppUserId && x.PostId == post.PostId); ;
        }

        public async Task<bool> CheckReservationExistsAsync(int id)
        {
            return await _context.Reservations.AnyAsync(x => x.ReservationId == id);
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
                    ReservationNote=reservation.ReservationNote,
                    ReservationId = reservation.ReservationId

                }).ToListAsync();
                
         
        }
    }
}
