using Course_Repository.Models;
using System.ComponentModel.DataAnnotations;

namespace Course_Repository.Services
{
    public class UserService : IUserService
    {
        private static List<User> _users = new();
        private static int _nextId = 1;
        private static readonly object _lockObject = new object();

        public Task<IEnumerable<User>> GetAllUsersAsync()
        {
            lock (_lockObject)
            {
                return Task.FromResult(_users.AsEnumerable());
            }
        }

        public Task<User?> GetUserByIdAsync(int id)
        {
            lock (_lockObject)
            {
                var user = _users.FirstOrDefault(u => u.Id == id);
                return Task.FromResult(user);
            }
        }

        public Task<(bool Success, User? User, string ErrorMessage)> CreateUserAsync(User user)
        {
            try
            {
                // Null check
                if (user == null)
                    return Task.FromResult((false, null as User, "User object cannot be null"));

                // Validate user properties
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(user);
                if (!Validator.TryValidateObject(user, validationContext, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    return Task.FromResult((false, null as User, errors));
                }

                lock (_lockObject)
                {
                    // Check for duplicate email
                    if (_users.Any(u => u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                        return Task.FromResult((false, null as User, "Email already exists"));

                    user.Id = _nextId++;
                    user.CreatedAt = DateTime.UtcNow;
                    user.UpdatedAt = DateTime.UtcNow;
                    _users.Add(user);
                    return Task.FromResult((true, (User?)user, string.Empty));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, null as User, $"An error occurred: {ex.Message}"));
            }
        }

        public Task<(bool Success, User? User, string ErrorMessage)> UpdateUserAsync(int id, User user)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                    return Task.FromResult((false, null as User, "Invalid user ID"));

                // Null check
                if (user == null)
                    return Task.FromResult((false, null as User, "User object cannot be null"));

                // Validate user properties
                var validationResults = new List<ValidationResult>();
                var validationContext = new ValidationContext(user);
                if (!Validator.TryValidateObject(user, validationContext, validationResults, true))
                {
                    var errors = string.Join(", ", validationResults.Select(r => r.ErrorMessage));
                    return Task.FromResult((false, null as User, errors));
                }

                lock (_lockObject)
                {
                    var existingUser = _users.FirstOrDefault(u => u.Id == id);
                    if (existingUser == null)
                        return Task.FromResult((false, null as User, "User not found"));

                    // Check for duplicate email (exclude current user)
                    if (_users.Any(u => u.Id != id && u.Email.Equals(user.Email, StringComparison.OrdinalIgnoreCase)))
                        return Task.FromResult((false, null as User, "Email already exists"));

                    // Only update if new values are provided (not empty)
                    if (!string.IsNullOrWhiteSpace(user.FirstName))
                        existingUser.FirstName = user.FirstName;
                    if (!string.IsNullOrWhiteSpace(user.LastName))
                        existingUser.LastName = user.LastName;
                    if (!string.IsNullOrWhiteSpace(user.Email))
                        existingUser.Email = user.Email;
                    if (!string.IsNullOrWhiteSpace(user.PhoneNumber))
                        existingUser.PhoneNumber = user.PhoneNumber;

                    existingUser.UpdatedAt = DateTime.UtcNow;
                    return Task.FromResult((true, (User?)existingUser, string.Empty));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, null as User, $"An error occurred: {ex.Message}"));
            }
        }

        public Task<(bool Success, string ErrorMessage)> DeleteUserAsync(int id)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                    return Task.FromResult((false, "Invalid user ID"));

                lock (_lockObject)
                {
                    var user = _users.FirstOrDefault(u => u.Id == id);
                    if (user == null)
                        return Task.FromResult((false, "User not found"));

                    _users.Remove(user);
                    return Task.FromResult((true, string.Empty));
                }
            }
            catch (Exception ex)
            {
                return Task.FromResult((false, $"An error occurred: {ex.Message}"));
            }
        }
    }
}
