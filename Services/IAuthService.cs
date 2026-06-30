using backend.DTOs.Auth;
using backend.Models;

namespace backend.Services;

public interface IAuthService
{
    Task<(bool Success, string? Error)> RegisterAsync(RegisterDto dto);
    Task<User?> ValidateLoginAsync(LoginDto dto);
    Task<User?> GetUserByIdAsync(int userId);
}
