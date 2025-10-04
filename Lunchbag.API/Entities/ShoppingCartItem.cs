using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lunchbag.API.Entities
{
    public class ShoppingCartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("ShoppingCartId")]
        public ShoppingCart? ShoppingCart { get; set; }

        public int ShoppingCartId { get; set; }

        [ForeignKey("ProductId")]
        public Product? Product { get; set; }
        public int ProductId { get; set; }

        [Required]
        public int Quantity { get; set; }
    }
}
