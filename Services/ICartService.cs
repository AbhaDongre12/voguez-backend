using backend.DTOs.Cart;
using backend.Models;

namespace backend.Services;

public interface ICartService
{
    Task<List<CartItems>> GetCartItemsAsync(int userId);
    Task<(bool Success, string? Error)> AddToCartAsync(int userId, AddToCartDto dto);
    Task<CartItems?> UpdateCartItemAsync(UpdateCartDto dto);
    Task<bool> RemoveFromCartAsync(int cartItemId);
}
