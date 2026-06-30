using backend.Models;

namespace backend.DTOs.Order;

public class UpdateOrderStatusDto
{
    public OrderStatus Status { get; set; } 
}
