using backend.DTOs.Profile;
using backend.Models;

namespace backend.Services;

public interface IAddressService
{
    Task<Address?> GetAddressAsync(int userId);
    Task UpdateAddressAsync(int userId, AddressDto dto);
}
