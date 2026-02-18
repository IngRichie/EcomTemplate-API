using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;


public class GuestUserService : IGuestUserService
{
    private readonly AppDbContext _db;

    public GuestUserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Guid> GetOrCreateGuestUserAsync(
    CheckoutRequestDTO request)
{
    if (string.IsNullOrWhiteSpace(request.DeviceId))
        throw new InvalidOperationException(
            "DeviceId is required for guest checkout.");

    // 🔎 Find existing guest by device
    var guest = await _db.GuestUsers
        .FirstOrDefaultAsync(g => g.DeviceId == request.DeviceId);

    if (guest == null)
    {
        // 🆕 Create new guest
        guest = new GuestUser
        {
            DeviceId = request.DeviceId,
            UniversityName = request.UniversityName,
            HostelName = request.HostelName,
            RoomNumber = request.RoomNumber,
            DeliveryInstructions = request.DeliveryInstructions
        };

        _db.GuestUsers.Add(guest);
    }
    else
    {
        // 🔁 Update delivery info if provided
        guest.UniversityName =
            request.UniversityName ?? guest.UniversityName;

        guest.HostelName =
            request.HostelName ?? guest.HostelName;

        guest.RoomNumber =
            request.RoomNumber ?? guest.RoomNumber;

        guest.DeliveryInstructions =
            request.DeliveryInstructions ?? guest.DeliveryInstructions;
    }

    await _db.SaveChangesAsync();
    return guest.GuestUserId;
}

}
