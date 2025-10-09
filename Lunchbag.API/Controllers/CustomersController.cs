using AutoMapper;
using Lunchbag.API.Entities;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Lunchbag.API.Controllers
{
    [Route("api/customers")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        ICustomerRepository _customerRepository;
        IMapper _mapper;
        IPasswordHasher<Customer> _passwordHasher;

        public CustomersController(ICustomerRepository customerRepository, IMapper mapper, IPasswordHasher<Customer> passwordHasher)
        {
            _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(_mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _customerRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<CustomerDto>>(customers));
        }

        [HttpGet("{id}", Name = "GetCustomer")]
        public async Task<ActionResult<CustomerDto>> GetCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if(customer == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CustomerDto>(customer));
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterCustomer(RegisterCustomerDto customer)
        {
            if (await _customerRepository.CustomerMailExistsAsync(customer.Email))
            {
                return BadRequest("Username/Email already exists.");
            }
            if (string.IsNullOrWhiteSpace(customer.Password))
            {
                return BadRequest("Password is required.");
            }

            var customerToRegister = _mapper.Map<Customer>(customer);

            customerToRegister.PasswordHash = _passwordHasher.HashPassword(customerToRegister,customer.Password);

            await _customerRepository.AddAsync(customerToRegister);
            await _customerRepository.SaveChangesAsync();

            var customerToReturn = _mapper.Map<CustomerDto>(customerToRegister);

            return CreatedAtRoute("GetCustomer", new
            {
                id = customerToReturn.Id,
            }, customerToReturn);
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginCustomer(LoginCustomerDto customer)
        {
            var customerToLogin = await _customerRepository.GetCustomerByEmail(customer.Email);

            if (customerToLogin == null)
            {
                return BadRequest("Username/Email doesn't exist.");
            }

            var result = _passwordHasher.VerifyHashedPassword(customerToLogin, customerToLogin.PasswordHash, customer.Password);
            
            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Wrong Password");
            }
                
            // TODO: Generate & return jwtToken
            //var token = jwtService.CreateToken(customer);
            //return Results.Ok(new { token });

            return Ok("Login Successfull");


        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateCustomer(int id, RegisterCustomerDto customer)
        {
            if (!await _customerRepository.ExistsAsync(id))
            {
                return NotFound();
            }

            var customerToUpdate = _mapper.Map<Customer>(customer);
            customerToUpdate.Id = id;

            customerToUpdate.PasswordHash = _passwordHasher.HashPassword(customerToUpdate, customer.Password);
            _customerRepository.Update(customerToUpdate);
            await _customerRepository.SaveChangesAsync();
            
            var customerToReturn = _mapper.Map<CustomerDto>(customerToUpdate);

            return CreatedAtRoute("GetCustomer",
                new
                {
                    id = customerToReturn.Id,
                }, customerToReturn);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteCustomer(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);

            if (customer == null)
            {
                return NotFound();
            }

            _customerRepository.Delete(customer);
            await _customerRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
