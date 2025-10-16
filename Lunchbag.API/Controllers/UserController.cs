using AutoMapper;
using Lunchbag.API.Entities;
using Lunchbag.API.Models;
using Lunchbag.API.Services;
using Microsoft.AspNetCore.Authorization;
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
        JwtService _jwtService;

        public UserController(IUserRepository userRepository, IMapper mapper, IPasswordHasher<User> passwordHasher, JwtService jwtService)
        {
            _userRepository = userRepository ?? throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
            _jwtService = jwtService ?? throw new ArgumentNullException(nameof(jwtService));
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

            var token = _jwtService.CreateToken(userToLogin);
            return Ok(new { token });
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
