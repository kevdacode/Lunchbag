using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lunchbag.API.Entities
{
    public class ShoppingCart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }

        public int CustomerId { get; set; }

        public ICollection<ShoppingCartItem> CartItems { get; set; } 
            = new List<ShoppingCartItem>();
    }
}
