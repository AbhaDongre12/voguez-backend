using backend.Models;

namespace backend.DTOs.Order;

public class OrderDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; } = "";
    public decimal TotalAmount { get; set; }
    public OrderStatus Status { get; set; } 
    public DateTime CreatedDate { get; set; }
    public string Address { get; set; } = "";
    public string City { get; set; } = "";
    public string PostalCode { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string OrderCode { get; set; } = "";

}
