using Microsoft.AspNetCore.Mvc;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Services;

namespace FoodOrderingAPI.Controllers
{
    /// <summary>
    /// API Controller for managing Contact Form Submissions
    /// Provides CRUD (Create, Read, Update, Delete) operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ContactsController : ControllerBase
    {
        private readonly IContactService _contactService;
        private readonly ILogger<ContactsController> _logger;

        public ContactsController(IContactService contactService, ILogger<ContactsController> logger)
        {
            _contactService = contactService;
            _logger = logger;
        }

        /// <summary>
        /// GET: api/contacts
        /// Retrieves all contact form submissions
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetAll()
        {
            _logger.LogInformation("Fetching all contact submissions");
            var contacts = await _contactService.GetAllAsync();
            return Ok(contacts);
        }

        /// <summary>
        /// GET: api/contacts/{id}
        /// Retrieves a specific contact submission by ID
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDto>> GetById(int id)
        {
            _logger.LogInformation("Fetching contact with ID: {Id}", id);
            var contact = await _contactService.GetByIdAsync(id);
            
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID {Id} not found", id);
                return NotFound(new { message = $"Contact with ID {id} not found" });
            }
            
            return Ok(contact);
        }

        /// <summary>
        /// GET: api/contacts/unread
        /// Retrieves all unread contact submissions
        /// </summary>
        [HttpGet("unread")]
        [ProducesResponseType(typeof(IEnumerable<ContactDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<ContactDto>>> GetUnread()
        {
            _logger.LogInformation("Fetching unread contact submissions");
            var contacts = await _contactService.GetUnreadAsync();
            return Ok(contacts);
        }

        /// <summary>
        /// POST: api/contacts
        /// Creates a new contact form submission
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ContactResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ContactResponseDto>> Create([FromBody] CreateContactDto dto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid contact form submission");
                return BadRequest(ModelState);
            }

            _logger.LogInformation("New contact form submission from: {Email}", dto.Email);
            var response = await _contactService.CreateAsync(dto);
            
            return CreatedAtAction(nameof(GetAll), response);
        }

        /// <summary>
        /// PUT: api/contacts/{id}/read
        /// Marks a contact submission as read
        /// </summary>
        [HttpPut("{id}/read")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDto>> MarkAsRead(int id)
        {
            _logger.LogInformation("Marking contact {Id} as read", id);
            var contact = await _contactService.MarkAsReadAsync(id);
            
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID {Id} not found", id);
                return NotFound(new { message = $"Contact with ID {id} not found" });
            }
            
            return Ok(contact);
        }

        /// <summary>
        /// PUT: api/contacts/{id}/resolve
        /// Marks a contact submission as resolved
        /// </summary>
        [HttpPut("{id}/resolve")]
        [ProducesResponseType(typeof(ContactDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ContactDto>> MarkAsResolved(int id)
        {
            _logger.LogInformation("Marking contact {Id} as resolved", id);
            var contact = await _contactService.MarkAsResolvedAsync(id);
            
            if (contact == null)
            {
                _logger.LogWarning("Contact with ID {Id} not found", id);
                return NotFound(new { message = $"Contact with ID {id} not found" });
            }
            
            return Ok(contact);
        }

        /// <summary>
        /// DELETE: api/contacts/{id}
        /// Deletes a contact submission
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation("Deleting contact with ID: {Id}", id);
            var result = await _contactService.DeleteAsync(id);
            
            if (!result)
            {
                _logger.LogWarning("Contact with ID {Id} not found for deletion", id);
                return NotFound(new { message = $"Contact with ID {id} not found" });
            }
            
            return NoContent();
        }
    }
}
