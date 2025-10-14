﻿namespace Lunchbag.API.Models
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }

        public bool Finalized { get; set; }

        public CustomerDto Customer { get; set; }
        public List<OrderItemDto> OrderItems { get; set; } = new();
    }
}
