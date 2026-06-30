using backend.DTOs.Category;
using backend.Models;

namespace backend.Services;

public interface ICategoryService
{
    Task<List<Category>> GetAllAsync();
    Task<Category?> GetByIdAsync(int id);
    Task<Category> CreateAsync(CategoryDto dto);
    Task<Category?> UpdateAsync(int id, CategoryDto dto);
    Task<bool> DeleteAsync(int id);
}
