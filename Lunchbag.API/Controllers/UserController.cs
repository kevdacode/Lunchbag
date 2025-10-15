using AutoMapper;
using Lunchbag.API.Entities;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


/**
 * TODO:
 * Add endpoints: 
 * api/customer/shoppingcart
 * api/customer/orders
 * */
namespace Lunchbag.API.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserRepository _userRepository;
        IMapper _mapper;
        IPasswordHasher<User> _passwordHasher;

        public UserController(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        }

        [HttpPost("register")]
        public async Task<ActionResult> RegisterCustomer(RegisterCustomerDto customer)
        {
            if (await _userRepository.UserEmailExistsAsync(customer.Email))
            {
                return BadRequest("Username/Email already exists.");
            }
            if (string.IsNullOrWhiteSpace(customer.Password))
            {
                return BadRequest("Password is required.");
            }

            var customerToRegister = _mapper.Map<Customer>(customer);
            customerToRegister.PasswordHash = _passwordHasher.HashPassword(customerToRegister,customer.Password);

            await _userRepository.AddAsync(customerToRegister);
            await _userRepository.SaveChangesAsync();

            return Ok("Registration Successfull");
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginUser(LoginUserDto user)
        {
            var userToLogin = await _userRepository.GetUserByEmail(user.Email);

            if (userToLogin == null)
            {
                return BadRequest("Username/Email doesn't exist.");
            }

            var result = _passwordHasher.VerifyHashedPassword(userToLogin, userToLogin.PasswordHash, user.Password);
            
            if (result != PasswordVerificationResult.Success)
            {
                return BadRequest("Wrong Password");
            }
                
            // TODO: Generate & return jwtToken
            //var token = jwtService.CreateToken(customer);
            //return Results.Ok(new { token });

            return Ok("Login Successfull");
        }

        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _userRepository.Delete(user);
            await _userRepository.SaveChangesAsync();
            return NoContent();
        }
    }
}
