using Microsoft.AspNetCore.Mvc;
using UrlShortener.Database;
using UrlShortener.Service;
using UrlShortener.Service.DTO;
using RegisterRequest = UrlShortener.Models.RegisterRequest;
using LoginRequest = UrlShortener.Models.LoginRequest;

namespace UrlShortener.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : Controller
    {
        private readonly DbStorageContext _context;
        private readonly UserService _userService;
        private readonly SessionService _sessionService;

        public UserController(DbStorageContext context, UserService userService, SessionService sessionService)
        {
            _context = context;
            _userService = userService;
            _sessionService = sessionService;
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest registerRequest)
        {
            if (string.IsNullOrEmpty(registerRequest.Name) || string.IsNullOrEmpty(registerRequest.Email) || string.IsNullOrEmpty(registerRequest.Password))
            {
                return BadRequest("Name, password, and token cannot be null or empty.");
            }

            if (await _userService.UserExists(registerRequest.Email))
            {
                return BadRequest("User with this token already exists.");
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
        [Route("account")]
        public async Task<IActionResult> GetUser()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Can not identify user");
            }

            var user = await _userService.GetUserByToken(token);

            if (user is not null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest("User not found.");
            }
        }
    }
}
