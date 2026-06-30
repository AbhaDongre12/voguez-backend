namespace backend.Models;

public class Address
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string PostalCode { get; set; } = string.Empty;

    public string PhoneNumber { get; set; } = string.Empty;

    public User User { get; set; } = null!;
}
