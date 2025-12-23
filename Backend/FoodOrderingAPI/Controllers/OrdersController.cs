using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Services;

namespace FoodOrderingAPI.Controllers
{
    /// <summary>
    /// API Controller for managing Orders
    /// Provides CRUD (Create, Read, Update, Delete) operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrdersController> _logger;

        public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/orders
        /// Retrieves all orders
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAll()
        {
            _logger.LogInformation("Fetching all orders");
            var orders = await _orderService.GetAllAsync();
            return Ok(orders);
        }

        /// <summary>
        /// GET: api/orders/{id}
        /// Retrieves a specific order by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetById(int id)
        {
            _logger.LogInformation("Fetching order with ID: {Id}", id);
            var order = await _orderService.GetByIdAsync(id);
            
            if (order == null)
            {
                _logger.LogWarning("Order with ID {Id} not found", id);
                return NotFound(new { message = $"Order with ID {id} not found" });
            }
            
            return Ok(order);
        }

        /// <summary>
        /// GET: api/orders/number/{orderNumber}
        /// Retrieves an order by order number
        /// </summary>
        [HttpGet("number/{orderNumber}")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<OrderDto>> GetByOrderNumber(string orderNumber)
        {
            _logger.LogInformation("Fetching order with number: {OrderNumber}", orderNumber);
            var order = await _orderService.GetByOrderNumberAsync(orderNumber);
            
            if (order == null)
            {
                _logger.LogWarning("Order with number {OrderNumber} not found", orderNumber);
                return NotFound(new { message = $"Order with number {orderNumber} not found" });
            }
            
            return Ok(order);
        }

        /// <summary>
        /// GET: api/orders/status/{status}
        /// Retrieves orders by status
        /// </summary>
        [HttpGet("status/{status}")]
        [ProducesResponseType(typeof(IEnumerable<OrderDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetByStatus(string status)
        {
            _logger.LogInformation("Fetching orders with status: {Status}", status);
            var orders = await _orderService.GetByStatusAsync(status);
            return Ok(orders);
        }

        /// <summary>
        /// POST: api/orders
        /// Creates a new order
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(OrderResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderResponseDto>> Create([FromBody] CreateOrderDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid order creation request");
                return BadRequest(ModelState);
            }

            if (dto.Items == null || !dto.Items.Any())
            {
                _logger.LogWarning("Order creation attempted with no items");
                return BadRequest(new { message = "Order must contain at least one item" });
            }

            _logger.LogInformation("Creating new order with {ItemCount} items", dto.Items.Count);
            var response = await _orderService.CreateAsync(dto);
            
            return CreatedAtAction(nameof(GetByOrderNumber), new { orderNumber = response.Id }, response);
        }

        /// <summary>
        /// PUT: api/orders/{id}/status
        /// Updates the status of an order
        /// </summary>
        [HttpPut("{id}/status")]
        [ProducesResponseType(typeof(OrderDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<OrderDto>> UpdateStatus(int id, [FromBody] UpdateOrderStatusDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Validate status
            var validStatuses = new[] { "Pending", "Confirmed", "Preparing", "OutForDelivery", "Delivered", "Cancelled" };
            if (!validStatuses.Contains(dto.Status))
            {
                return BadRequest(new { message = $"Invalid status. Valid statuses are: {string.Join(", ", validStatuses)}" });
            }

            _logger.LogInformation("Updating order {Id} status to: {Status}", id, dto.Status);
            var order = await _orderService.UpdateStatusAsync(id, dto.Status);
            
            if (order == null)
            {
                _logger.LogWarning("Order with ID {Id} not found for status update", id);
                return NotFound(new { message = $"Order with ID {id} not found" });
            }
            
            return Ok(order);
        }

        /// <summary>
        /// DELETE: api/orders/{id}
        /// Deletes an order
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting order with ID: {Id}", id);
            var result = await _orderService.DeleteAsync(id);
            
            if (!result)
            {
                _logger.LogWarning("Order with ID {Id} not found for deletion", id);
                return NotFound(new { message = $"Order with ID {id} not found" });
            }
            
            return NoContent();
        }
    }
}
