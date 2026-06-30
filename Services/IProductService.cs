using backend.DTOs.Product;
using backend.Models;

namespace backend.Services;

public interface IProductService
{
    Task<List<Product>> GetAllAsync(string? category, string? search, string? sort);
    Task<Product> GetByIdAsync(int id);
    Task<Product> CreateAsync(ProductDto dto);
    Task<Product?> UpdateAsync(int id, ProductDto dto);
    Task<bool> DeleteAsync(int id);
}
