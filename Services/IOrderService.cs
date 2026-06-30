using backend.DTOs.Order;
using backend.Models;

namespace backend.Services;

public interface IOrderService
{
    Task<List<OrderDto>> GetAllOrdersAsync();
    Task<Order?> UpdateStatusAsync(int id, UpdateOrderStatusDto dto);
    Task<(bool Success, string? Error, object? Result)> CreateOrderAsync(int userId);
    Task<object> GetMyOrdersAsync(int userId);
    Task<(bool Success, string? Error)> CancelOrderAsync(int userId, int orderId);
}
