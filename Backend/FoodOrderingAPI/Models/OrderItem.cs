using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderingAPI.Models
{
    /// <summary>
    /// Represents an item within an order
    /// </summary>
    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        [Required]
        [StringLength(100)]
        public string ItemName { get; set; } = string.Empty;

        [Required]
        public int Quantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(10, 2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal TotalPrice => UnitPrice * Quantity;

        [StringLength(500)]
        public string SpecialInstructions { get; set; } = string.Empty;

        // Navigation properties
        [ForeignKey("OrderId")]
        public Order? Order { get; set; }

        [ForeignKey("MenuItemId")]
        public MenuItem? MenuItem { get; set; }
    }
}
