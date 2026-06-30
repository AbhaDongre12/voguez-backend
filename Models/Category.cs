using System.Text.Json.Serialization;

namespace backend.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string ImageUrl { get; set; } = null!;
        [JsonIgnore]
        public ICollection<Product> Products { get; set; }=new List<Product>();
    }
}
