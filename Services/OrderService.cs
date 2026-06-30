using backend.Data;
using backend.DTOs.Order;
using backend.Models;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class OrderService : IOrderService
{
    private readonly AppDbContext _context;

    public OrderService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<OrderDto>> GetAllOrdersAsync()
    {
        return await _context.Orders.Include(o => o.User)
            .OrderByDescending(o => o.CreatedDate)
            .Select(o => new OrderDto
            {
                Id = o.Id,
                UserId = o.UserId,
                Name = o.User.Name,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                CreatedDate = o.CreatedDate
            })
            .ToListAsync();
    }

    public async Task<Order?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
            return null;

        order.Status = dto.Status;
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task<(bool Success, string? Error, object? Result)> CreateOrderAsync(int userId)
    {
        var cartItems = await _context.CartItems
            .Include(c => c.Product)
            .Include(c => c.Cart)
            .Where(c => c.Cart.UserId == userId)
            .ToListAsync();

        if (!cartItems.Any())
            return (false, "Cart is empty", null);

        var total = cartItems.Sum(
            item => item.Product.Price * item.Quantity
        );

        var order = new Order
        {
            UserId = userId,
            OrderCode = GenerateOrderCode(),
            TotalAmount = total,
            Status = OrderStatus.Pending
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        foreach (var cartItem in cartItems)
        {
            var orderItem = new OrderItems
            {
                OrderId = order.Id,
                ProductId = cartItem.ProductId,
                Quantity = cartItem.Quantity,
                Price = cartItem.Product.Price
            };
            _context.OrderItems.Add(orderItem);
        }

        _context.CartItems.RemoveRange(cartItems);
        await _context.SaveChangesAsync();

        return (true, null, new
        {
            order.Id,
            order.OrderCode,
            order.TotalAmount,
            Status = order.Status.ToString()
        });
    }

    public async Task<object> GetMyOrdersAsync(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
            .OrderByDescending(o => o.Id)
            .Select(o => new
            {
                o.Id,
                o.OrderCode,
                o.TotalAmount,
                Status = o.Status.ToString(),
                o.CreatedDate,
                OrderItems = o.OrderItems.Select(oi => new
                {
                    oi.Id,
                    oi.ProductId,
                    oi.Quantity,
                    oi.Price,
                    Product = new
                    {
                        oi.Product.Id,
                        oi.Product.Name,
                        oi.Product.ImageUrl
                    }
                })
            })
            .ToListAsync();
    }

    public async Task<(bool Success, string? Error)> CancelOrderAsync(int userId, int orderId)
    {
        var order = await _context.Orders
            .FirstOrDefaultAsync(o =>
                o.Id == orderId &&
                o.UserId == userId);

        if (order == null)
            return (false, null);

        if (order.Status != OrderStatus.Pending)
            return (false, "Only pending orders can be cancelled");

        order.Status = OrderStatus.Cancelled;
        await _context.SaveChangesAsync();
        return (true, null);
    }

    private static string GenerateOrderCode()
    {
        return $"ORD-{Random.Shared.Next(100000, 999999)}";
    }
}
