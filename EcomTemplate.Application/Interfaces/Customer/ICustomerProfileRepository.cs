using EcomTemplate.Domain.Entities;

namespace EcomTemplate.Application.Interfaces;
public interface ICustomerProfileRepository
{
    Task<CustomerProfile?> GetByIdAsync(Guid customerId);
    Task<CustomerProfile> CreateAsync(CustomerProfile profile);

    Task UpdateAsync(CustomerProfile profile);
    Task DeleteAsync(Guid customerId);
}
