using Microsoft.AspNetCore.Mvc;
using UrlShortener.Database;
using UrlShortener.Service;
using UrlShortener.Service.DTO;
using RegisterRequest = UrlShortener.Models.RegisterRequest;

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

            var userData = new UserData(registerRequest.Name, registerRequest.Email, registerRequest.Password)
            {
                Name = registerRequest.Name,
                Email = registerRequest.Email,
                Password = registerRequest.Password
            };

            var result = await _userService.CreateUser(userData);

            if (result)
            {
                return Ok("User created successfully.");
            }
            else
            {
                return BadRequest("User creation failed.");
            }
        }
    }
}
