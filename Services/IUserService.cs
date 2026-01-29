using Course_Repository.Models;

namespace Course_Repository.Services
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id);
        Task<(bool Success, User? User, string ErrorMessage)> CreateUserAsync(User user);
        Task<(bool Success, User? User, string ErrorMessage)> UpdateUserAsync(int id, User user);
        Task<(bool Success, string ErrorMessage)> DeleteUserAsync(int id);
    }
}
