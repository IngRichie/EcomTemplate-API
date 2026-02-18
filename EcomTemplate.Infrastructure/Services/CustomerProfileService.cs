using AutoMapper;
using GrocerySupermarket.Application.DTOs;
using GrocerySupermarket.Application.Interfaces;
using GrocerySupermarket.Domain.Entities;
using GrocerySupermarket.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class CustomerProfileService
{
    private readonly ICustomerProfileRepository _repo;
    private readonly IMapper _mapper;
    private readonly AppDbContext _dbContext;

    public CustomerProfileService(
        ICustomerProfileRepository repo,
        IMapper mapper,
        AppDbContext appContext)
    {
        _repo = repo;
        _mapper = mapper;
        _dbContext = appContext;
    }

    public async Task<CustomerDTO?> GetProfileAsync(Guid customerId)
    {
        var profile = await _repo.GetByIdAsync(customerId);
        return profile == null ? null : _mapper.Map<CustomerDTO>(profile);
    }

    public async Task<CustomerDTO> CreateOrUpdateAsync(
        Guid customerId,
        UpdateCustomerProfileDTO dto)
    {
        var profile = await _repo.GetByIdAsync(customerId);

        if (profile == null)
        {
            profile = new CustomerProfile
            {
                CustomerProfileId = customerId
            };
            _mapper.Map(dto, profile);
            await _repo.CreateAsync(profile);
        }
        else
        {
            _mapper.Map(dto, profile);
            await _repo.UpdateAsync(profile);
        }

        return _mapper.Map<CustomerDTO>(profile);
    }

    public async Task DeleteAsync(Guid customerId)
    {
        await _repo.DeleteAsync(customerId);
    }

    public async Task UpdateProfileImageAsync(
    Guid customerId,
    string imageUrl)
{
    var profile = await _dbContext.CustomerProfiles.FirstOrDefaultAsync(c => c.CustomerProfileId == customerId);

    if (profile == null)
        throw new InvalidOperationException("Customer profile not found.");

    profile.ProfileImageUrl = imageUrl;

    await _dbContext.SaveChangesAsync();
}




}
