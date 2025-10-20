using AutoMapper;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Lunchbag.API.Controllers
{
    [Authorize(Policy = "AdminOnly")]
    [Route("api/admin")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public AdminController(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetAllOrders()
        {
            var orders = await _orderRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("order/{id}", Name = "GetOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if(order == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpPatch("order/{id}/finalize")]
        public async Task<ActionResult> FinalizeOrder(int id, UpdateOrderFinalizedDto updateOrderFinalizedDto)
        {
            var orderToFinalize = await _orderRepository.GetByIdAsync(id);

            if (orderToFinalize == null)
            {
                return NotFound();
            }

            orderToFinalize.Finalized = updateOrderFinalizedDto.Finalized;
            await _orderRepository.SaveChangesAsync();

            var savedOrder = await _orderRepository.GetByIdAsync(id);
            var orderToReturn = _mapper.Map<OrderDto>(savedOrder);

            return Ok(orderToReturn);
        }

        [HttpDelete("order/{id}")]
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
