namespace Lunchbag.API.Models
{
    public class OrderForCreationDto
    {
        public DateTime OrderDate { get; set; } = DateTime.Now;
        public int CustomerId { get; set; }
        public List<OrderItemForCreationDto> OrderItems { get; set; } = new();
    }
}
