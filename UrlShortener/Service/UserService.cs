using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Entities;
using UrlShortener.Service.DTO;

namespace UrlShortener.Service
{
    public class UserService
    {
        private readonly DbStorageContext _context;

        public UserService(DbStorageContext context)
        {
            _context = context;
        }

        public async Task<bool> RegisterUser(RegisterRequestData registerRequestData)
        {
            if (await UserExists(registerRequestData.Email))
            {
                return false;
            }

            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = registerRequestData.Name,
                Password = PasswordHashing.HashPassword(registerRequestData.Password),
                Email = registerRequestData.Email,
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(r => r.Email == email);
        }

        public async Task<Guid?> Login(LoginRequestData loginRequestData)
        {
            var user = await _context.Users.SingleOrDefaultAsync(r => r.Email == loginRequestData.Email);

            if (user is not null && PasswordHashing.VerifyPassword(user.Password, loginRequestData.Password))
            {
                var sessionService = new SessionService(_context);
                var sessionKey = await sessionService.CreateSession(user.Email);

                return sessionKey;
            }

            return null;
        }

        public async Task<bool> Logout(string email)
        {
            var sessionService = new SessionService(_context);

            return await sessionService.DeleteSession(email);
        }

        public async Task<bool> ValidateUser(LoginRequestData loginRequestData)
        {
            var user = await _context.Users.SingleOrDefaultAsync(r => r.Email == loginRequestData.Email);

            if (user is null || !PasswordHashing.VerifyPassword(user.Password, loginRequestData.Password))
            {
                return false;
            }

            return true;
        }
    }
}
