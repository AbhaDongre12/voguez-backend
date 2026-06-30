using backend.Data;
using backend.DTOs.Cart;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class CartService : ICartService
{
    private readonly AppDbContext _context;

    public CartService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<CartItems>> GetCartItemsAsync(int userId)
    {
        return await _context.CartItems
            .Include(c => c.Product)
            .Where(c => c.Cart.UserId == userId)
            .ToListAsync();
    }

    public async Task<(bool Success, string? Error)> AddToCartAsync(int userId, AddToCartDto dto)
    {
        var product = await _context.Products.FindAsync(dto.ProductId);
        if (product == null)
            return (false, "Product Not Found");

        if (product.Quantity < dto.Quantity)
            return (false, "Insufficient Stock");

        var cart = await _context.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);

        if (cart == null)
        {
            cart = new Cart
            {
                UserId = userId
            };
            _context.Carts.Add(cart);
            await _context.SaveChangesAsync();
        }

        var existingItems = await _context.CartItems
            .FirstOrDefaultAsync(c =>
                c.CartId == cart.Id &&
                c.ProductId == dto.ProductId);

        if (existingItems != null)
        {
            existingItems.Quantity += dto.Quantity;
        }
        else
        {
            _context.CartItems.Add(
                new CartItems
                {
                    CartId = cart.Id,
                    ProductId = dto.ProductId,
                    Quantity = dto.Quantity
                });
        }

        await _context.SaveChangesAsync();
        return (true, null);
    }

    public async Task<CartItems?> UpdateCartItemAsync(UpdateCartDto dto)
    {
        var item = await _context.CartItems.FindAsync(dto.CartItemId);
        if (item == null)
            return null;

        item.Quantity = dto.Quantity;
        await _context.SaveChangesAsync();
        return item;
    }

    public async Task<bool> RemoveFromCartAsync(int cartItemId)
    {
        var item = await _context.CartItems.FindAsync(cartItemId);
        if (item == null)
            return false;

        _context.CartItems.Remove(item);
        await _context.SaveChangesAsync();
        return true;
    }
}
