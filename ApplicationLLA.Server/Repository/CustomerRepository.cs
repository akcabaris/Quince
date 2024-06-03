using ApplicationLLA.Server.DBC;
using ApplicationLLA.Server.Dtos.Customer;
using ApplicationLLA.Server.Interfaces;
using ApplicationLLA.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace ApplicationLLA.Server.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ApplicationDBContext _context;

        public CustomerRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<Customer?> CreateAsync(string id)
        {
            var customer = new Customer
            {
                AppUserId = id,
                FullName = string.Empty,
                PhoneNumber = 5000000000L,
            };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            if (customer == null) { return null; }

            return customer;
        }

        public async Task<Customer?> GetByUserIdAsync(string id)
        {
            var customer = await _context.Customers.FirstOrDefaultAsync(x => x.AppUserId == id);
            return customer;
        }

        public Task<bool> IsCustomerExistsAsync(string id)
        {
            return _context.Customers.AnyAsync(x => x.AppUserId == id);
        }

        public async Task<Customer?> UpdateAsync(string id, UpdateCustomerDto customerDto)
        {
            if (customerDto == null) return null;

            var existCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.AppUserId == id);
            if (existCustomer == null) return null;

            existCustomer.FullName = customerDto.FullName;
            existCustomer.PhoneNumber = customerDto.PhoneNumber;
            existCustomer.Gender = customerDto.Gender;

            await _context.SaveChangesAsync();
            return existCustomer;
        }


        public async Task<Customer?> UpdateProfilePicAsync(string id, string createdImageName)
        {
            if (createdImageName == null) return null;

            var existsCustomer = await _context.Customers.FirstOrDefaultAsync(x => x.AppUserId == id);
            if (existsCustomer == null) return null;

            existsCustomer.PictureLink = createdImageName;

            await _context.SaveChangesAsync();
            return existsCustomer;
        }

        public async Task<bool> DeleteProfilePicAsync(string id)
        {
            var existsCustomer = await _context.Workers.FirstOrDefaultAsync(x => x.AppUserId == id);
            if (existsCustomer == null) return false;

            existsCustomer.PictureLink = null;
            await _context.SaveChangesAsync();

            return (existsCustomer.PictureLink == null);
        }
    }
}
