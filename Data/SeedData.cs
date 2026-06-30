using backend.Models;
using BCrypt.Net;

namespace backend.Data;

public static class SeedData
{
    public static void Initialize(AppDbContext context)
    {
        context.Database.EnsureCreated();
        //Admin user
        if (!context.Users.Any())
        {
            context.Users.Add(new User
            {
                Name = "Admin",
                Email = "admin@fashioncommerce.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                Role = "Admin",
            });
            context.SaveChanges();
        }
        //categories
        if(!context.Categories.Any()) 
        {
            var categories = new List<Category>
            {
                new Category {Name="Men",ImageUrl="/images/categories/men.jpg"},
                new Category {Name="Women",ImageUrl="/images/categories/women.jpg"},
                new Category {Name="Footwear",ImageUrl="/images/categories/shoes.jpeg"},
                new Category {Name="Accessories",ImageUrl="/images/categories/accessories.jpg"},
                new Category {Name="Sale",ImageUrl="/images/categories/sale.jpg"}
            };
            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        //products
        if (!context.Products.Any())
        {
            context.Products.AddRange(
                new Product
                {
                    Name = "White Oxford Shirt",
                    Description = "Premium cotton shirt",
                    Price = 1499,
                    Quantity = 50,
                    ImageUrl = "/images/categories/whiteoxfordshirt.webp",
                    CategoryId = 1
                },

                new Product
                {
                    Name = "Slim Fit Jeans",
                    Description = "Blue denim",
                    Price = 1999,
                    Quantity = 30,
                    ImageUrl = "/images/categories/slimfitjeans.webp",
                    CategoryId = 1
                },

                new Product
                {
                    Name = "Summer Dress",
                    Description = "Floral dress",
                    Price = 2499,
                    Quantity = 20,
                    ImageUrl = "/images/categories/summerdress.webp",
                    CategoryId = 2
                },

                new Product
                {
                    Name = "Pendant Necklace",
                    Description = "Gold chain with seashell pendant",
                    Price = 200,
                    Quantity = 40,
                    ImageUrl = "/images/categories/pendantnecklace.webp",
                    CategoryId = 4
                },

                new Product
                {
                    Name = "Sneakers",
                    Description = "White sneakers with gold laces",
                    Price = 1399,
                    Quantity = 15,
                    ImageUrl = "/images/categories/whitesneakers.webp",
                    CategoryId = 3
                }

             );
            context.SaveChanges();
        }
    }
}
