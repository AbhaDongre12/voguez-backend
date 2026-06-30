namespace backend.Services;

public interface IAIService
{
    Task<object> ProcessQueryAsync(int userId, string message);
}
