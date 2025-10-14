using AutoMapper;
using Lunchbag.API.Entities;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Lunchbag.API.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public OrdersController(IOrderRepository orderRepository, IProductRepository productRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpPost]
        public async Task<ActionResult> CreateOrder(OrderForCreationDto order)
        {
            var orderToAdd = _mapper.Map<Order>(order);
            foreach(OrderItem o in orderToAdd.OrderItems)
            {
                var product = await _productRepository.GetByIdAsync(o.ProductId);

                if (product == null || product.Stock < o.Quantity) 
                {
                    return BadRequest("Insufficient stock.");
                }
                o.UnitPrice = product.Price;
                orderToAdd.TotalAmount += o.Quantity * o.UnitPrice;
            }

            await _orderRepository.AddAsync(orderToAdd);
            await _orderRepository.SaveChangesAsync();

            var savedOrder = await _orderRepository.GetByIdAsync(orderToAdd.Id);
            var orderToReturn = _mapper.Map<OrderDto>(savedOrder);
            return CreatedAtRoute("GetOrder", new
            {
                id = orderToReturn.Id,
            }, orderToReturn);

        }

        [HttpPatch("{id}/finalize")]
        public async Task<ActionResult> FinalizeOrder(int id, UpdateOrderFinalizedDto updateOrderFinalizedDto)
        {
            var orderToFinalize = await _orderRepository.GetByIdAsync(id);

            if (orderToFinalize == null)
            {
                return NotFound();
            }

            orderToFinalize.Finalized = updateOrderFinalizedDto.Finalized;
 
            _orderRepository.Update(orderToFinalize);
            await _orderRepository.SaveChangesAsync();

            var savedOrder = await _orderRepository.GetByIdAsync(id);
            var orderToReturn = _mapper.Map<OrderDto>(savedOrder);

            return Ok(orderToReturn);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) { 
                return NotFound();
            }

            _orderRepository.Delete(order);
            await _orderRepository.SaveChangesAsync();

            return NoContent();
        }
    }
}
