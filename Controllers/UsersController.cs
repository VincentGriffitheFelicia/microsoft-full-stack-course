using Course_Repository.Models;
using Course_Repository.Services;
using Microsoft.AspNetCore.Mvc;

namespace Course_Repository.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>List of all users</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetAllUsers()
        {
            try
            {
                var users = await _userService.GetAllUsersAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users");
                return StatusCode(500, new { message = "An error occurred while retrieving users", error = ex.Message });
            }
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="id">User ID</param>
        /// <returns>User object if found</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                {
                    _logger.LogInformation("Log: User ID must be greater than 0");
                    return BadRequest(new { message = "User ID must be greater than 0" });
                }
                    

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                    return NotFound(new { message = $"User with ID {id} not found" });

                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving user with ID {id}");
                return StatusCode(500, new { message = "An error occurred while retrieving the user", error = ex.Message });
            }
        }

        /// <summary>
        /// Create a new user
        /// </summary>
        /// <param name="user">User object to create</param>
        /// <returns>Created user object</returns>
        [HttpPost]
        public async Task<ActionResult<User>> CreateUser([FromBody] User? user)
        {
            try
            {
                // Null check
                if (user == null)
                    return BadRequest(new { error = "User object cannot be null" });

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .Where(m => !string.IsNullOrWhiteSpace(m))
                        .ToList();
                    return BadRequest(new { error = "Validation failed", details = errors });
                }

                var (success, createdUser, errorMessage) = await _userService.CreateUserAsync(user);
                if (!success)
                    return BadRequest(new { error = "Failed to create user", details = errorMessage });

                return CreatedAtAction(nameof(GetUserById), new { id = createdUser?.Id }, createdUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating user");
                return StatusCode(500, new { error = "Internal server error", details = ex.Message });
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="id">User ID to update</param>
        /// <param name="user">Updated user data</param>
        /// <returns>Updated user object</returns>
        [HttpPut("{id}")]
        public async Task<ActionResult<User>> UpdateUser(int id, [FromBody] User? user)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                    return BadRequest(new { message = "User ID must be greater than 0" });

                // Null check
                if (user == null)
                    return BadRequest(new { message = "User object cannot be null" });

                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .Where(m => !string.IsNullOrWhiteSpace(m))
                        .ToList();
                    return BadRequest(new { error = "Validation failed", details = errors });
                }

                var (success, updatedUser, errorMessage) = await _userService.UpdateUserAsync(id, user);
                if (!success)
                    return NotFound(new { error = "Failed to update user", details = errorMessage });

                return Ok(updatedUser);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating user with ID {id}");
                return StatusCode(500, new { message = "An error occurred while updating the user", error = ex.Message });
            }
        }

        /// <summary>
        /// Delete a user
        /// </summary>
        /// <param name="id">User ID to delete</param>
        /// <returns>Success or not found response</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            try
            {
                // Validate ID
                if (id <= 0)
                    return BadRequest(new { message = "User ID must be greater than 0" });

                var (success, errorMessage) = await _userService.DeleteUserAsync(id);
                if (!success)
                    return NotFound(new { message = errorMessage });

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting user with ID {id}");
                return StatusCode(500, new { message = "An error occurred while deleting the user", error = ex.Message });
            }
        }
    }
}
