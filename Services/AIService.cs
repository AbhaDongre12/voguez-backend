using backend.Data;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class AIService : IAIService
{
    private readonly GeminiService _geminiService;
    private readonly AppDbContext _context;

    public AIService(GeminiService geminiService, AppDbContext context)
    {
        _geminiService = geminiService;
        _context = context;
    }

    public async Task<object> ProcessQueryAsync(int userId, string message)
    {
        var intent = await _geminiService.GetIntentAsync(message);

        switch (intent)
        {
            case "GET_LAST_ORDER":
                var lastOrder = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedDate)
                    .FirstOrDefaultAsync();
                return lastOrder!;

            case "GET_CART":
                var cartItems = await _context.CartItems
                    .Include(c => c.Product)
                    .Include(c => c.Cart)
                    .Where(c => c.Cart.UserId == userId)
                    .ToListAsync();

                if (!cartItems.Any())
                    return "Your cart is empty.";

                return new
                {
                    Items = cartItems.Select(i => new
                    {
                        ProductName = i.Product.Name,
                        Quantity = i.Quantity,
                        Price = i.Product.Price
                    }),
                    Total = cartItems.Sum(i => i.Quantity * i.Product.Price)
                };

            case "GET_PENDING_ORDERS":
                var pendingOrders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o =>
                        o.UserId == userId &&
                        o.Status == OrderStatus.Pending
                    )
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();
                return pendingOrders;

            case "GET_ALL_ORDERS":
                var allOrders = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                    .Where(o => o.UserId == userId)
                    .OrderByDescending(o => o.CreatedDate)
                    .ToListAsync();
                return allOrders;

            default:
                return new
                {
                    Message = "Sorry I could not understand your request."
                };
        }
    }
}
