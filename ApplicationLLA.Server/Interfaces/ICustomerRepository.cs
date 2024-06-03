using ApplicationLLA.Server.Dtos.Customer;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Interfaces
{
    public interface ICustomerRepository
    {
        Task<Customer?> GetByUserIdAsync(string id);
        Task<Customer?> CreateAsync(string id);
        Task<bool> IsCustomerExistsAsync(string id);
        Task<Customer?> UpdateAsync(string id, UpdateCustomerDto customerDto);

        Task<Customer?> UpdateProfilePicAsync(string id, string createdImageName);
        Task<bool> DeleteProfilePicAsync(string id);

    }
}
