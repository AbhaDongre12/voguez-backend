using System.ComponentModel.DataAnnotations;

namespace backend.DTOs.Order;

public class CreateOrderDto
{
    [Required]
    public string Address { get; set; } = "";
    [Required]
    public string City { get; set; } = "";
    [Required]
    [RegularExpression(@"^\d{5,10}$")]
    public string PostalCode { get; set; } = "";
    [Required]
    public string PhoneNumber { get; set; } = "";
}
