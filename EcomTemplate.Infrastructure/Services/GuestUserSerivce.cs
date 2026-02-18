using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Infrastructure.Data;
using GrocerySupermarket.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace GrocerySupermarket.Infrastructure.Repositories
{
    public class GuestUserService : IGuestUserService
    {
        private readonly AppDbContext _db;

        public GuestUserService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<Guid> GetOrCreateGuestUserAsync(CheckoutRequestDTO request)
        {
            if (request.GuestUserId.HasValue)
                return request.GuestUserId.Value;

            if (string.IsNullOrWhiteSpace(request.DeviceId))
                throw new InvalidOperationException("DeviceId is required for guest checkout.");

            // Try to find existing guest user by device ID
            var guest = await _db.GuestUsers.FirstOrDefaultAsync(g => g.DeviceId == request.DeviceId);
            if (guest != null)
                return guest.GuestUserId;

            // Create new guest user
            guest = new GuestUser
            {
                DeviceId = request.DeviceId,
                CreatedAt = DateTime.UtcNow
            };

            await _db.GuestUsers.AddAsync(guest);
            await _db.SaveChangesAsync();

            return guest.GuestUserId;
        }
    }
}
