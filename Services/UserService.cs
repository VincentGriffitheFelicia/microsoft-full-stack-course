using Course_Repository.Models;

namespace Course_Repository.Services
{
    public class UserService : IUserService
    {
        private static List<User> _users = new();
        private static int _nextId = 1;

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return Task.FromResult(user);
        }

        public Task<User> CreateUserAsync(User user)
        {
            user.Id = _nextId++;
            user.CreatedAt = DateTime.UtcNow;
            user.UpdatedAt = DateTime.UtcNow;
            _users.Add(user);
            return Task.FromResult(user);
        }

        public Task<User?> UpdateUserAsync(int id, User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id);
            if (existingUser == null)
                return Task.FromResult<User?>(null);

            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Email = user.Email;
            existingUser.PhoneNumber = user.PhoneNumber;
            existingUser.UpdatedAt = DateTime.UtcNow;

            return Task.FromResult<User?>(existingUser);
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            if (user == null)
                return Task.FromResult(false);

            _users.Remove(user);
            return Task.FromResult(true);
        }
    }
}
