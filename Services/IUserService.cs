namespace backend.Services;

public interface IUserService
{
    Task<object> GetAllUsersAsync();
}
