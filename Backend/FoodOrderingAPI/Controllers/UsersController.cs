using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Services;

namespace FoodOrderingAPI.Controllers
{
    /// <summary>
    /// Controller for user authentication and management
    /// </summary>
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
        /// Login or register a user
        /// </summary>
        /// <param name="dto">Login credentials (name and email)</param>
        /// <returns>User information if successful</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponseDto>> Login([FromBody] LoginDto dto)
        {
            _logger.LogInformation("Login/Register attempt for email: {Email}", dto.Email);

            if (!ModelState.IsValid)
            {
                return BadRequest(new LoginResponseDto
                {
                    Success = false,
                    Message = "Please provide valid name and email.",
                    User = null
                });
            }

            var result = await _userService.LoginOrRegisterAsync(dto);

            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

        /// <summary>
        /// Get all users
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            _logger.LogInformation("Fetching all users");
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(int id)
        {
            _logger.LogInformation("Fetching user with ID: {Id}", id);
            var user = await _userService.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Get user by email
        /// </summary>
        [HttpGet("email/{email}")]
        public async Task<ActionResult<UserDto>> GetByEmail(string email)
        {
            _logger.LogInformation("Fetching user with email: {Email}", email);
            var user = await _userService.GetByEmailAsync(email);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        [HttpPut("{id}")]
        public async Task<ActionResult<UserDto>> Update(int id, [FromBody] UpdateUserDto dto)
        {
            _logger.LogInformation("Updating user with ID: {Id}", id);
            var user = await _userService.UpdateAsync(id, dto);

            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            return Ok(user);
        }

        /// <summary>
        /// Get all orders for a specific user
        /// </summary>
        [HttpGet("{id}/orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetUserOrders(int id)
        {
            _logger.LogInformation("Fetching orders for user ID: {Id}", id);
            
            // First check if user exists
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
            {
                return NotFound(new { message = "User not found" });
            }

            var orders = await _userService.GetUserOrdersAsync(id);
            return Ok(orders);
        }

        /// <summary>
        /// Delete user (soft delete)
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting user with ID: {Id}", id);
            var result = await _userService.DeleteAsync(id);

            if (!result)
            {
                return NotFound(new { message = "User not found" });
            }

            return NoContent();
        }
    }
}
