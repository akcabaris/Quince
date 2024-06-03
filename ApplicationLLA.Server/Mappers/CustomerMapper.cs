using ApplicationLLA.Server.Dtos.Customer;
using ApplicationLLA.Server.Dtos.Worker;
using ApplicationLLA.Server.Models;

namespace ApplicationLLA.Server.Mappers
{
    public static class CustomerMapper
    {
        public static CustomerDto ToCustomerDto (this Customer customer)
        {
            return new CustomerDto
            {
                CustomerId =customer.AppUserId,
                FullName = customer.FullName,
                PhoneNumber = customer.PhoneNumber,
                Gender = customer.Gender,
                PictureLink = "http://localhost:5279/resources/" + customer.PictureLink,
            };
        }

        public static Customer ToCustomerFromUpdate (this UpdateCustomerDto customerDto, string id)
        {
            return new Customer
            {
                AppUserId = id,
                FullName = customerDto.FullName,
                Gender = customerDto.Gender,
                PhoneNumber = customerDto.PhoneNumber,
            };
        }
    }
}
