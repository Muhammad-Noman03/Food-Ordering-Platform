using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Services
{
    public interface IMenuItemService
    {
        Task<IEnumerable<MenuItemDto>> GetAllAsync();
        Task<MenuItemDto?> GetByIdAsync(int id);
        Task<IEnumerable<MenuItemDto>> GetByCategoryAsync(string category);
        Task<IEnumerable<MenuItemDto>> GetPopularAsync();
        Task<IEnumerable<MenuItemDto>> SearchAsync(string searchTerm);
        Task<MenuItemDto> CreateAsync(CreateMenuItemDto dto);
        Task<MenuItemDto?> UpdateAsync(int id, UpdateMenuItemDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<OrderDto?> GetByIdAsync(int id);
        Task<OrderDto?> GetByOrderNumberAsync(string orderNumber);
        Task<IEnumerable<OrderDto>> GetByStatusAsync(string status);
        Task<OrderResponseDto> CreateAsync(CreateOrderDto dto);
        Task<OrderDto?> UpdateStatusAsync(int id, string status);
        Task<bool> DeleteAsync(int id);
    }

    public interface IContactService
    {
        Task<IEnumerable<ContactDto>> GetAllAsync();
        Task<ContactDto?> GetByIdAsync(int id);
        Task<IEnumerable<ContactDto>> GetUnreadAsync();
        Task<ContactResponseDto> CreateAsync(CreateContactDto dto);
        Task<ContactDto?> MarkAsReadAsync(int id);
        Task<ContactDto?> MarkAsResolvedAsync(int id);
        Task<bool> DeleteAsync(int id);
    }

    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<UserDto?> GetByEmailAsync(string email);
        Task<LoginResponseDto> LoginOrRegisterAsync(LoginDto dto);
        Task<UserDto?> UpdateAsync(int id, UpdateUserDto dto);
        Task<IEnumerable<OrderDto>> GetUserOrdersAsync(int userId);
        Task<bool> DeleteAsync(int id);
    }
}
