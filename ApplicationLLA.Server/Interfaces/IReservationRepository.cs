using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateAsync(Reservation reservModel);
        Task<Reservation?> DeleteAsync(int id);
        Task<List<Reservation>> GetUsersReservationsAsync(Customer customer);
        Task<Reservation?> GetByIdAsync(int id);
        Task<Reservation?> ApproveOrDenyReservationAsync(int id, string status);
        Task<bool> CheckReservationExistsAsync(Customer customer, Post post);
        Task<bool> CheckReservationExistsAsync(int id);
    }
}
