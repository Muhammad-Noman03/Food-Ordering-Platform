using Microsoft.EntityFrameworkCore;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationDbContext context, ILogger<UserService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<UserDto>> GetAllAsync()
        {
            var users = await _context.Users
                .Where(u => u.IsActive)
                .OrderByDescending(u => u.CreatedAt)
                .ToListAsync();

            return users.Select(MapToDto);
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<UserDto?> GetByEmailAsync(string email)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower() && u.IsActive);
            return user != null ? MapToDto(user) : null;
        }

        public async Task<LoginResponseDto> LoginOrRegisterAsync(LoginDto dto)
        {
            try
            {
                // Check if user exists by email
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.Email.ToLower() == dto.Email.ToLower());

                if (existingUser != null)
                {
                    // User exists - update last login and return
                    existingUser.LastLoginAt = DateTime.UtcNow;
                    
                    // Update name, phone, address if provided
                    if (!string.IsNullOrEmpty(dto.FullName))
                        existingUser.FullName = dto.FullName;
                    if (!string.IsNullOrEmpty(dto.Phone))
                        existingUser.Phone = dto.Phone;
                    if (!string.IsNullOrEmpty(dto.Address))
                        existingUser.Address = dto.Address;

                    await _context.SaveChangesAsync();

                    _logger.LogInformation("User logged in: {Email}", dto.Email);

                    return new LoginResponseDto
                    {
                        Success = true,
                        Message = "Welcome back!",
                        User = MapToDto(existingUser)
                    };
                }
                else
                {
                    // Create new user
                    var newUser = new User
                    {
                        FullName = dto.FullName,
                        Email = dto.Email.ToLower(),
                        Phone = dto.Phone ?? string.Empty,
                        Address = dto.Address ?? string.Empty,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        LastLoginAt = DateTime.UtcNow
                    };

                    _context.Users.Add(newUser);
                    await _context.SaveChangesAsync();

                    _logger.LogInformation("New user registered: {Email}", dto.Email);

                    return new LoginResponseDto
                    {
                        Success = true,
                        Message = "Account created successfully! Welcome to FoodieExpress!",
                        User = MapToDto(newUser)
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login/register for email: {Email}", dto.Email);
                return new LoginResponseDto
                {
                    Success = false,
                    Message = "An error occurred. Please try again.",
                    User = null
                };
            }
        }

        public async Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return null;

            if (!string.IsNullOrEmpty(dto.FullName))
                user.FullName = dto.FullName;
            if (!string.IsNullOrEmpty(dto.Phone))
                user.Phone = dto.Phone;
            if (!string.IsNullOrEmpty(dto.Address))
                user.Address = dto.Address;

            await _context.SaveChangesAsync();
            return MapToDto(user);
        }

        public async Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(o => new OrderDto
            {
                Id = o.Id,
                OrderNumber = o.OrderNumber,
                CustomerName = o.CustomerName,
                CustomerEmail = o.CustomerEmail,
                CustomerPhone = o.CustomerPhone,
                DeliveryAddress = o.DeliveryAddress,
                TotalAmount = o.TotalAmount,
                Status = o.Status,
                Notes = o.Notes,
                OrderDate = o.OrderDate,
                Items = o.OrderItems.Select(oi => new OrderItemDto
                {
                    MenuItemId = oi.MenuItemId,
                    ItemName = oi.ItemName,
                    Quantity = oi.Quantity,
                    Price = oi.UnitPrice,
                    SpecialInstructions = oi.SpecialInstructions
                }).ToList()
            });
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return false;

            // Soft delete - just mark as inactive
            user.IsActive = false;
            await _context.SaveChangesAsync();
            return true;
        }

        private static UserDto MapToDto(User user)
        {
            return new UserDto
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Phone = user.Phone,
                Address = user.Address,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt
            };
        }
    }
}
