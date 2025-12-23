using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Services;

namespace FoodOrderingAPI.Controllers
{
    /// <summary>
    /// API Controller for managing Menu Items
    /// Provides CRUD (Create, Read, Update, Delete) operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class MenuItemsController : ControllerBase
    {
        private readonly IMenuItemService _menuItemService;
        private readonly ILogger<MenuItemsController> _logger;

        public MenuItemsController(IMenuItemService menuItemService, ILogger<MenuItemsController> logger)
        {
            _menuItemService = menuItemService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/menuitems
        /// Retrieves all available menu items
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetAll()
        {
            _logger.LogInformation("Fetching all menu items");
            var items = await _menuItemService.GetAllAsync();
            return Ok(items);
        }

        /// <summary>
        /// GET: api/menuitems/{id}
        /// Retrieves a specific menu item by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<MenuItemDto>> GetById(int id)
        {
            _logger.LogInformation("Fetching menu item with ID: {Id}", id);
            var item = await _menuItemService.GetByIdAsync(id);
            
            if (item == null)
            {
                _logger.LogWarning("Menu item with ID {Id} not found", id);
                return NotFound(new { message = $"Menu item with ID {id} not found" });
            }
            
            return Ok(item);
        }

        /// <summary>
        /// GET: api/menuitems/category/{category}
        /// Retrieves menu items by category
        /// </summary>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetByCategory(string category)
        {
            _logger.LogInformation("Fetching menu items in category: {Category}", category);
            var items = await _menuItemService.GetByCategoryAsync(category);
            return Ok(items);
        }

        /// <summary>
        /// GET: api/menuitems/popular
        /// Retrieves popular menu items
        /// </summary>
        [HttpGet("popular")]
        [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> GetPopular()
        {
            _logger.LogInformation("Fetching popular menu items");
            var items = await _menuItemService.GetPopularAsync();
            return Ok(items);
        }

        /// <summary>
        /// GET: api/menuitems/search?term={searchTerm}
        /// Searches menu items by name, description, or category
        /// </summary>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<MenuItemDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<MenuItemDto>>> Search([FromQuery] string term)
        {
            if (string.IsNullOrWhiteSpace(term))
            {
                return await GetAll();
            }

            _logger.LogInformation("Searching menu items with term: {Term}", term);
            var items = await _menuItemService.SearchAsync(term);
            return Ok(items);
        }

        /// <summary>
        /// POST: api/menuitems
        /// Creates a new menu item
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MenuItemDto>> Create([FromBody] CreateMenuItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid menu item creation request");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Creating new menu item: {Name}", dto.Name);
            var item = await _menuItemService.CreateAsync(dto);
            
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }

        /// <summary>
        /// PUT: api/menuitems/{id}
        /// Updates an existing menu item
        /// </summary>
        [HttpPut("{id}")]
        [ProducesResponseType(typeof(MenuItemDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<MenuItemDto>> Update(int id, [FromBody] UpdateMenuItemDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Updating menu item with ID: {Id}", id);
            var item = await _menuItemService.UpdateAsync(id, dto);
            
            if (item == null)
            {
                _logger.LogWarning("Menu item with ID {Id} not found for update", id);
                return NotFound(new { message = $"Menu item with ID {id} not found" });
            }
            
            return Ok(item);
        }

        /// <summary>
        /// DELETE: api/menuitems/{id}
        /// Deletes a menu item
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting menu item with ID: {Id}", id);
            var result = await _menuItemService.DeleteAsync(id);
            
            if (!result)
            {
                _logger.LogWarning("Menu item with ID {Id} not found for deletion", id);
                return NotFound(new { message = $"Menu item with ID {id} not found" });
            }
            
            return NoContent();
        }
    }
}
