using backend.DTOs.Profile;

namespace backend.Services;

public interface IProfileService
{
    Task<bool> UpdateProfileAsync(int userId, UpdateProfileDto dto);
    Task<(bool Success, string? Message)> ChangePasswordAsync(int userId, ChangePasswordDto dto);
}
