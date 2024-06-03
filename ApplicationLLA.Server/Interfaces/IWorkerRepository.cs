using ApplicationLLA.Server.Dtos.Worker;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface IWorkerRepository
    {
        Task<Worker?> GetByUserIdAsync(string id);
        Task<Worker?> CreateAsync(string id);
        Task<bool> IsWorkerExistsAsync(string id);
        Task<Worker?> UpdateAsync(string id, UpdateWorkerDto workerDto);
        Task<Worker?> UpdateProfilePicAsync(string id, string createdImageName);
        Task<bool> DeleteProfilePicAsync(string id);
    }
}
