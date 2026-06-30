using backend.Data;
using Microsoft.EntityFrameworkCore;

namespace backend.Services;

public class DashboardService : IDashboardService
{
    private readonly AppDbContext _context;

    public DashboardService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<(int TotalUsers, int TotalProducts, int TotalOrders, decimal Revenue)> GetStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalProducts = await _context.Products.CountAsync();
        var totalOrders = await _context.Orders.CountAsync();
        var revenue = await _context.Orders.SumAsync(o => o.TotalAmount);
        return (totalUsers, totalProducts, totalOrders, revenue);
    }
}
