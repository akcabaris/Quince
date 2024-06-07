using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> CreateAsync(Reservation reservModel);
        Task<Reservation?> DeleteAsync(int id);
        Task<List<Reservation>> GetUsersReservationsAsync(Customer customer);
        Task<Reservation?> GetByIdAsync(int id);
        Task<bool> CheckIsOwnerRight(string userId, int reservationId);
        Task<Reservation?> ApproveOrDenyReservationAsync(int id, string status);
        Task<string> GetReservationStatus(string customerId, int postId);
        Task<string> GetReservationStatusById(int reservation);
        Task <bool> CheckIsReservationExists(string userId, int postId);
    }
}
