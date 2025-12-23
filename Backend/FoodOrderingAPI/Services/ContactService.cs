using Microsoft.EntityFrameworkCore;
using FoodOrderingAPI.Data;
using FoodOrderingAPI.DTOs;
using FoodOrderingAPI.Models;

namespace FoodOrderingAPI.Services
{
    /// <summary>
    /// Service for managing contact form submissions (CRUD operations)
    /// </summary>
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ContactService> _logger;

        public ContactService(ApplicationDbContext context, ILogger<ContactService> logger)
        {
            _context = context;
            _logger = logger;
        }

        // READ: Get all contacts
        public async Task<IEnumerable<ContactDto>> GetAllAsync()
        {
            var contacts = await _context.Contacts
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return contacts.Select(MapToDto);
        }

        // READ: Get contact by ID
        public async Task<ContactDto?> GetByIdAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            return contact == null ? null : MapToDto(contact);
        }

        // READ: Get unread contacts
        public async Task<IEnumerable<ContactDto>> GetUnreadAsync()
        {
            var contacts = await _context.Contacts
                .Where(c => !c.IsRead)
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();

            return contacts.Select(MapToDto);
        }

        // CREATE: Submit new contact form
        public async Task<ContactResponseDto> CreateAsync(CreateContactDto dto)
        {
            var contact = new Contact
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.Email,
                Phone = dto.Phone ?? string.Empty,
                Subject = dto.Subject,
                Message = dto.Message,
                Newsletter = dto.Newsletter,
                IsRead = false,
                IsResolved = false,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Contacts.Add(contact);
            await _context.SaveChangesAsync();

            _logger.LogInformation("New contact form submitted from: {Email} (ID: {Id})", contact.Email, contact.Id);

            return new ContactResponseDto
            {
                Success = true,
                Message = "Thank you for your message! We'll get back to you soon."
            };
        }

        // UPDATE: Mark contact as read
        public async Task<ContactDto?> MarkAsReadAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return null;
            }

            contact.IsRead = true;
            contact.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Marked contact as read: {Id}", id);

            return MapToDto(contact);
        }

        // UPDATE: Mark contact as resolved
        public async Task<ContactDto?> MarkAsResolvedAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return null;
            }

            contact.IsResolved = true;
            contact.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation("Marked contact as resolved: {Id}", id);

            return MapToDto(contact);
        }

        // DELETE: Delete contact
        public async Task<bool> DeleteAsync(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null)
            {
                return false;
            }

            _context.Contacts.Remove(contact);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Deleted contact: {Id}", id);

            return true;
        }

        // Helper method to map entity to DTO
        private static ContactDto MapToDto(Contact contact)
        {
            return new ContactDto
            {
                Id = contact.Id,
                FirstName = contact.FirstName,
                LastName = contact.LastName,
                Email = contact.Email,
                Phone = contact.Phone,
                Subject = contact.Subject,
                Message = contact.Message,
                Newsletter = contact.Newsletter,
                IsRead = contact.IsRead,
                IsResolved = contact.IsResolved,
                CreatedAt = contact.CreatedAt
            };
        }
    }
}
