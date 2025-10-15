namespace Lunchbag.API.Models
{
    public class ShoppingCartForCreationDto
    {
        public int CustomerId { get; set; }
        public List<ShoppingCartItemForCreationDto> CartItems { get; set; } = new();
    }
}
