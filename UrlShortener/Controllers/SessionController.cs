using Microsoft.AspNetCore.Mvc;
using UrlShortener.Database;
using UrlShortener.Service;
using UrlShortener.Service.DTO;
using LoginRequest = UrlShortener.Models.LoginRequest;
namespace UrlShortener.Controllers
{
    public class SessionController : Controller
    {
        private readonly DbStorageContext _context;
        private readonly UserService _userService;
        private readonly SessionService _sessionService;

        public SessionController(DbStorageContext context, UserService userService, SessionService sessionService)
        {
            _context = context;
            _userService = userService;
            _sessionService = sessionService;
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
                return BadRequest("User with this token doesn't exist.");
            }

            var loginRequestData = new LoginRequestData()
            {
                Email = loginRequest.Email,
                Password = loginRequest.Password
            };

            var loginResult = await _sessionService.Login(loginRequestData);

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
        public async Task<IActionResult> Logout()
        {
            var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Can not identify user");
            }

            var result = await _sessionService.Logout(token);

            if (result)
            {
                return Ok("User logged out successfully.");
            }
            else
            {
                return BadRequest("User logout failed.");
            }
        }
    }
}
