using AutoMapper;
using Lunchbag.API.Entities;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;


namespace Lunchbag.API.Controllers
{
    [Authorize(Policy = "CustomerOnly")]
    [Route("api/customer")]
    [ApiController]
    public class CustomerController : ControllerBase
    {

        private readonly ICustomerRepository _customerRepository;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerRepository customerRepository, IProductRepository productRepository, IMapper mapper)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet("shoppingcart")]
        public async Task<ActionResult> GetShoppingCart()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shoppingCart = await _customerRepository.GetCustomerShoppingCartAsync(int.Parse(customerId));
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpPost("shoppingcart/add")]
        public async Task<ActionResult> AddItemToShoppingCart(ShoppingCartItemForCreationDto item)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if(product == null)
            {
                return BadRequest($"Product with ID {item.ProductId} has no matching Product in the database.");
            }

            if(product.Stock < item.Quantity)
            {
                return BadRequest($"Insufficient stock for product '{product.Name}'.");
            }

            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shoppingCart = await _customerRepository.GetCustomerShoppingCartAsync(int.Parse(customerId));
            var cartItems = shoppingCart.CartItems;

            var itemToIncrease = cartItems.FirstOrDefault(i => i.ProductId == item.ProductId);

            if (itemToIncrease == null)
            {
                cartItems.Add(_mapper.Map<ShoppingCartItem>(item));
            }
            else
            {
                if (product.Stock < itemToIncrease.Quantity + item.Quantity)
                {
                    return BadRequest($"Insufficient stock for product '{product.Name}'.");
                }
                itemToIncrease.Quantity += item.Quantity;
            }
            await _customerRepository.SaveChangesAsync();
            shoppingCart = await _customerRepository.GetCustomerShoppingCartAsync(int.Parse(customerId));
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpPost("shoppingcart/delete")]
        public async Task<ActionResult> DeleteItemFromShoppingCart(ShoppingCartItemForCreationDto item)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId);
            if (product == null)
            {
                return BadRequest($"Product with ID {item.ProductId} has no matching Product in the database.");
            }

            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shoppingCart = await _customerRepository.GetCustomerShoppingCartAsync(int.Parse(customerId));
            var cartItems = shoppingCart.CartItems;

            var itemToDecrease = cartItems.FirstOrDefault(i => i.ProductId == item.ProductId);


            if(itemToDecrease == null)
            {
                return BadRequest($"Product with ID {item.ProductId} is not in the shopping cart.");
            }

            itemToDecrease.Quantity -= item.Quantity;

            if (itemToDecrease.Quantity < 1)
            {
                cartItems.Remove(itemToDecrease);
            }

            await _customerRepository.SaveChangesAsync();
            shoppingCart = await _customerRepository.GetCustomerShoppingCartAsync(int.Parse(customerId));
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpPost("shoppingcart/finalize")]
        public async Task<ActionResult> FinalizeShoppingCart()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var shoppingCart = await _customerRepository.GetCustomerShoppingCartAsync(int.Parse(customerId));

            var order = new Order();

            try
            {
                order = await ConvertCartToOrder(shoppingCart);
            }
            catch (Exception ex) 
            { 
                return BadRequest(ex.Message);
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(int.Parse(customerId));
            customer.Orders.Add(order);
            shoppingCart.CartItems.Clear();
            await _customerRepository.SaveChangesAsync();
            var orderToReturn = _mapper.Map<OrderDto>(order);

            return CreatedAtRoute("GetCustomerOrder", new
            {
                id = order.Id,
            }, orderToReturn);
        }

        private async Task<Order> ConvertCartToOrder(ShoppingCart cart)
        {
            var order = new Order
            {
                CustomerId = cart.CustomerId,
                OrderDate = DateTime.UtcNow,
                Finalized = false,
                OrderItems = new List<OrderItem>()
            };

            decimal totalAmount = 0;

            foreach (var cartItem in cart.CartItems)
            {
                var product = await _productRepository.GetByIdAsync(cartItem.ProductId);

                if (product == null)
                {
                    throw new Exception($"CartItem with ID {cartItem.Id} has no matching Product in the database.");
                }

                if (product.Stock < cartItem.Quantity)
                {
                    var oldQuantity = cartItem.Quantity;
                    cartItem.Quantity = product.Stock;

                    await _customerRepository.SaveChangesAsync();

                    throw new Exception(
                        $"Insufficient stock for product '{product.Name}'. " +
                        $"Requested: {oldQuantity}, Available: {product.Stock}. " +
                        $"Cart quantity adjusted to {product.Stock}.");
                }

                var orderItem = new OrderItem
                {
                    ProductId = cartItem.ProductId,
                    Quantity = cartItem.Quantity,
                    UnitPrice = product.Price,
                };

                totalAmount += orderItem.UnitPrice * orderItem.Quantity;

                order.OrderItems.Add(orderItem);
                
                product.Stock -= cartItem.Quantity;
            }

            order.TotalAmount = totalAmount;
            await _productRepository.SaveChangesAsync();
            return order;
        }

        [HttpGet("order/{id}", Name = "GetCustomerOrder")]
        public async Task<ActionResult<OrderDto>> GetOrder(int id)
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _customerRepository.GetCustomerOrdersAsync(int.Parse(customerId));

            var order = orders.Where(o => o.Id == id).FirstOrDefault();

            if(order == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<OrderDto>(order));
        }


        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrders()
        {
            var customerId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var orders = await _customerRepository.GetCustomerOrdersAsync(int.Parse(customerId));

            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }
    }
}
