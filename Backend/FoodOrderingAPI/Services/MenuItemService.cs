using Microsoft.EntityFrameworkCore;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Services
{
    /// <summary>
    /// Service for managing menu items (CRUD operations)
    /// </summary>
    public class MenuItemService : IMenuItemService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<MenuItemService> _logger;

        public MenuItemService(ApplicationDbContext context, ILogger<MenuItemService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // READ: Get all menu items
        public async Task<IEnumerable<MenuItemDto>> GetAllAsync()
        {
            var items = await _context.MenuItems
                .Where(m => m.IsAvailable)
                .OrderBy(m => m.Category)
                .ThenBy(m => m.Name)
                .ToListAsync();

            return items.Select(MapToDto);
        }

        // READ: Get menu item by ID
        public async Task<MenuItemDto?> GetByIdAsync(int id)
        {
            var item = await _context.MenuItems.FindAsync(id);
            return item == null ? null : MapToDto(item);
        }

        // READ: Get menu items by category
        public async Task<IEnumerable<MenuItemDto>> GetByCategoryAsync(string category)
        {
            var items = await _context.MenuItems
                .Where(m => m.Category.ToLower() == category.ToLower() && m.IsAvailable)
                .OrderBy(m => m.Name)
                .ToListAsync();

            return items.Select(MapToDto);
        }

        // READ: Get popular menu items
        public async Task<IEnumerable<MenuItemDto>> GetPopularAsync()
        {
            var items = await _context.MenuItems
                .Where(m => m.IsPopular && m.IsAvailable)
                .OrderByDescending(m => m.Rating)
                .Take(8)
                .ToListAsync();

            return items.Select(MapToDto);
        }

        // READ: Search menu items
        public async Task<IEnumerable<MenuItemDto>> SearchAsync(string searchTerm)
        {
            var term = searchTerm.ToLower();
            var items = await _context.MenuItems
                .Where(m => m.IsAvailable && 
                    (m.Name.ToLower().Contains(term) || 
                     m.Description.ToLower().Contains(term) ||
                     m.Category.ToLower().Contains(term)))
                .OrderBy(m => m.Name)
                .ToListAsync();

            return items.Select(MapToDto);
        }

        // CREATE: Add new menu item
        public async Task<MenuItemDto> CreateAsync(CreateMenuItemDto dto)
        {
            var menuItem = new MenuItem
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Category = dto.Category,
                Image = dto.Image,
                Rating = dto.Rating,
                IsPopular = dto.IsPopular,
                IsAvailable = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.MenuItems.Add(menuItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new menu item: {Name} (ID: {Id})", menuItem.Name, menuItem.Id);

            return MapToDto(menuItem);
        }

        // UPDATE: Update existing menu item
        public async Task<MenuItemDto?> UpdateAsync(int id, UpdateMenuItemDto dto)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return null;
            }

            // Update only provided fields
            if (dto.Name != null) menuItem.Name = dto.Name;
            if (dto.Description != null) menuItem.Description = dto.Description;
            if (dto.Price.HasValue) menuItem.Price = dto.Price.Value;
            if (dto.Category != null) menuItem.Category = dto.Category;
            if (dto.Image != null) menuItem.Image = dto.Image;
            if (dto.Rating.HasValue) menuItem.Rating = dto.Rating.Value;
            if (dto.IsPopular.HasValue) menuItem.IsPopular = dto.IsPopular.Value;
            if (dto.IsAvailable.HasValue) menuItem.IsAvailable = dto.IsAvailable.Value;

            menuItem.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated menu item: {Name} (ID: {Id})", menuItem.Name, menuItem.Id);

            return MapToDto(menuItem);
        }

        // DELETE: Remove menu item
        public async Task<bool> DeleteAsync(int id)
        {
            var menuItem = await _context.MenuItems.FindAsync(id);
            if (menuItem == null)
            {
                return false;
            }

            _context.MenuItems.Remove(menuItem);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted menu item: {Name} (ID: {Id})", menuItem.Name, menuItem.Id);

            return true;
        }

        // Helper method to map entity to DTO
        private static MenuItemDto MapToDto(MenuItem item)
        {
            return new MenuItemDto
            {
                Id = item.Id,
                Name = item.Name,
                Description = item.Description,
                Price = item.Price,
                Category = item.Category,
                Image = item.Image,
                Rating = item.Rating,
                IsPopular = item.IsPopular,
                IsAvailable = item.IsAvailable
            };
        }
    }
}
