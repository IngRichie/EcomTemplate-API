using EcomTemplate.Infrastructure.Data;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Domain.Interfaces;

using Microsoft.EntityFrameworkCore;

namespace GrocerySupermarket.Infrastructure.Repositories;

public class ContactMessageRepository : IContactMessageRepository
{
    private readonly AppDbContext _db;

    public ContactMessageRepository(AppDbContext context)
    {
        _db = context;
    }

    public async Task<ContactMessage> CreateAsync(ContactMessage contactMessage)
    {
        await _db.ContactMessage.AddAsync(contactMessage);
        await _db.SaveChangesAsync();

        return contactMessage;
    }

    public async Task<IEnumerable<ContactMessage>> GetAllAsync()
    {
        return await _db.ContactMessage
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    public async Task<ContactMessage?> GetByIdAsync(Guid contactMessageId)
    {
        return await _db.ContactMessage
            .FirstOrDefaultAsync(x =>
                x.ContactMessageId == contactMessageId);
    }

    public async Task<bool> DeleteAsync(Guid contactMessageId)
    {
        var contact = await _db.ContactMessage
            .FirstOrDefaultAsync(x =>
                x.ContactMessageId == contactMessageId);

        if (contact is null)
            return false;

        _db.ContactMessage.Remove(contact);

        await _db.SaveChangesAsync();

        return true;
    }
}