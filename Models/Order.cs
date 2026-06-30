namespace backend.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OrderCode { get; set; } = "";
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; } 
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public User? User { get; set; }
        public ICollection<OrderItems> OrderItems { get; set; }=new List<OrderItems>();
    }
}
