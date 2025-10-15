namespace Lunchbag.API.Models
{
    public class ShoppingCartDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public List<ShoppingCartItemDto> CartItems { get; set; } = new();
    }
}
