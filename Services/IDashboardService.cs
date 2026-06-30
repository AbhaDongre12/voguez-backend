namespace backend.Services;

public interface IDashboardService
{
    Task<(int TotalUsers, int TotalProducts, int TotalOrders, decimal Revenue)> GetStatsAsync();
}
