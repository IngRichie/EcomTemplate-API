using GrocerySupermarket.Domain.Entities;

namespace GrocerySupermarket.Domain.Interfaces;

public interface IContactMessageRepository
{
    Task<ContactMessage> CreateAsync(ContactMessage contactMessage);

    Task<IEnumerable<ContactMessage>> GetAllAsync();

    Task<ContactMessage?> GetByIdAsync(Guid contactMessageId);

    Task<bool> DeleteAsync(Guid contactMessageId);
}