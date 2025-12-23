using Microsoft.EntityFrameworkCore;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Services
{
    /// <summary>
    /// Service for managing orders (CRUD operations)
    /// </summary>
    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<OrderService> _logger;

        public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // READ: Get all orders
        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(MapToDto);
        }

        // READ: Get order by ID
        public async Task<OrderDto?> GetByIdAsync(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            return order == null ? null : MapToDto(order);
        }

        // READ: Get order by order number
        public async Task<OrderDto?> GetByOrderNumberAsync(string orderNumber)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.OrderNumber == orderNumber);

            return order == null ? null : MapToDto(order);
        }

        // READ: Get orders by status
        public async Task<IEnumerable<OrderDto>> GetByStatusAsync(string status)
        {
            var orders = await _context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.Status.ToLower() == status.ToLower())
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            return orders.Select(MapToDto);
        }

        // CREATE: Create new order
        public async Task<OrderResponseDto> CreateAsync(CreateOrderDto dto)
        {
            // Generate unique order number
            var orderNumber = $"ORD-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..8].ToUpper()}";

            var order = new Order
            {
                OrderNumber = orderNumber,
                UserId = dto.UserId,  // Link to user if logged in
                CustomerName = dto.CustomerName,
                CustomerEmail = dto.CustomerEmail,
                CustomerPhone = dto.CustomerPhone,
                DeliveryAddress = dto.DeliveryAddress,
                Notes = dto.Notes,
                TotalAmount = dto.TotalAmount,
                Status = OrderStatus.Confirmed,
                OrderDate = dto.OrderDate != default ? dto.OrderDate : DateTime.UtcNow,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // Add order items
            foreach (var itemDto in dto.Items)
            {
                var orderItem = new OrderItem
                {
                    MenuItemId = itemDto.MenuItemId,
                    ItemName = itemDto.ItemName,
                    Quantity = itemDto.Quantity,
                    UnitPrice = itemDto.Price,
                    SpecialInstructions = itemDto.SpecialInstructions
                };
                order.OrderItems.Add(orderItem);
            }

            // Recalculate total if not provided
            if (order.TotalAmount == 0)
            {
                order.TotalAmount = order.OrderItems.Sum(i => i.UnitPrice * i.Quantity);
            }

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Created new order: {OrderNumber} (ID: {Id})", order.OrderNumber, order.Id);

            return new OrderResponseDto
            {
                Id = order.OrderNumber,
                Status = order.Status,
                Message = "Order placed successfully!"
            };
        }

        // UPDATE: Update order status
        public async Task<OrderDto?> UpdateStatusAsync(int id, string status)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return null;
            }

            order.Status = status;
            order.UpdatedAt = DateTime.UtcNow;

            if (status == OrderStatus.Delivered)
            {
                order.DeliveryDate = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Updated order status: {OrderNumber} -> {Status}", order.OrderNumber, status);

            return MapToDto(order);
        }

        // DELETE: Delete order
        public async Task<bool> DeleteAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return false;
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted order: {OrderNumber} (ID: {Id})", order.OrderNumber, order.Id);

            return true;
        }

        // Helper method to map entity to DTO
        private static OrderDto MapToDto(Order order)
        {
            return new OrderDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                CustomerName = order.CustomerName,
                CustomerEmail = order.CustomerEmail,
                CustomerPhone = order.CustomerPhone,
                DeliveryAddress = order.DeliveryAddress,
                TotalAmount = order.TotalAmount,
                Status = order.Status,
                Notes = order.Notes,
                OrderDate = order.OrderDate,
                Items = order.OrderItems.Select(i => new OrderItemDto
                {
                    MenuItemId = i.MenuItemId,
                    ItemName = i.ItemName,
                    Quantity = i.Quantity,
                    Price = i.UnitPrice,
                    SpecialInstructions = i.SpecialInstructions
                }).ToList()
            };
        }
    }
}
