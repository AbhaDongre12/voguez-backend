using backend.Data;
using backend.DTOs.Profile;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class AddressService : IAddressService
{
    private readonly AppDbContext _context;

    public AddressService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Address?> GetAddressAsync(int userId)
    {
        return await _context.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId);
    }

    public async Task UpdateAddressAsync(int userId, AddressDto dto)
    {
        var address = await _context.Addresses
            .FirstOrDefaultAsync(a => a.UserId == userId);

        if (address == null)
        {
            address = new Address
            {
                UserId = userId
            };
            _context.Addresses.Add(address);
        }

        address.Street = dto.Street;
        address.City = dto.City;
        address.PostalCode = dto.PostalCode;
        address.PhoneNumber = dto.PhoneNumber;

        await _context.SaveChangesAsync();
    }
}
