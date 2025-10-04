using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lunchbag.API.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Stock { get; set; }

        [ForeignKey("CategoryId")]
        public Category? Category { get; set; }

        public int CategoryId { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
        public ICollection<ShoppingCartItem> CartItems { get; set; } = new List<ShoppingCartItem>();
    }
}
