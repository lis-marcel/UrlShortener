using Microsoft.AspNetCore.Mvc;
using UrlShortener.Database;
using UrlShortener.Service;
using UrlShortener.Service.DTO;
using RegisterRequest = UrlShortener.Models.RegisterRequest;
using LoginRequest = UrlShortener.Models.LoginRequest;

namespace UrlShortener.Controllers
{
    public class UserController : Controller
    {
        private readonly DbStorageContext _context;
        private readonly UserService _userService;

        public UserController(DbStorageContext context, UserService userService)
        {
            _context = context;
            _userService = userService;
        }

        [HttpPost]
        [Route("/register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Name) || string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return BadRequest("Name, password, and email cannot be null or empty.");
            }

            if (await _userService.UserExists(registerRequest.Email))
            {
                return BadRequest("User with this email already exists.");
            }

            var registerRequestData = new RegisterRequestData()
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            var result = await _userService.RegisterUser(registerRequestData);

            if (result)
            {
                return Ok("User created successfully.");
            }
            else
            {
                return BadRequest("User creation failed.");
            }
        }

        [HttpPost]
        [Route("/login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            if (string.IsNullOrEmpty(loginRequest.Email) || string.IsNullOrEmpty(loginRequest.Password))
            {
                return BadRequest("Email and password cannot be null or empty.");
            }

            if (!await _userService.UserExists(loginRequest.Email))
            {
                return BadRequest("User with this email doesn't exist.");
            }

            var loginRequestData = new LoginRequestData()
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password
            };

            var loginResult = await _userService.Login(loginRequestData);

            if (loginResult is not null)
            {
                return Ok(loginResult);
            }
            else
            {
                return BadRequest("Invalid email or password.");
            }
        }

        [HttpPost]
        [Route("/logout")]
        public async Task<IActionResult> Logout([FromBody] string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest("Can not identify user");
            }

            var result = await _userService.Logout(email);

            if (result)
            {
                return Ok("User logged out successfully.");
            }
            else
            {
                return BadRequest("User logout failed.");
            }
        }

        [HttpPost]
    }
}
