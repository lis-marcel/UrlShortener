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
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = registerRequestData.Name,
                Password = PasswordHashing.HashPassword(registerRequestData.Password),
                Email = registerRequestData.Email,
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        public async Task<Guid?> Login(LoginRequestData loginRequestData)
        {
            if (await UserExists(loginRequestData.Email) && await ValidateUser(loginRequestData))
            {
                return await CreateSession(loginRequestData.Email);
            }

            return null;
        }

        public async Task<bool> UserExists(string email)
        {
            return await _context.Users.AnyAsync(r => r.Email == email);
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

        public async Task<Guid> CreateSession(string email)
        {
            var session = new Session(email);

            if (await SessionExists(email))
            {
                var existingSession = _context.Sessions.Single(r => r.Email == email);
                existingSession.SessionKey = Guid.NewGuid();
                existingSession.CreationTime = DateTime.Now;

                _context.Sessions.Update(existingSession);
            }
            else
            {
                _context.Sessions.Add(session);
            }

            await _context.SaveChangesAsync();

            return session.SessionKey;
        }

        private async Task<bool> SessionExists(string email)
        {
            var session = await _context.Sessions.SingleOrDefaultAsync(r => r.Email == email);

            return (session is not null) ? true : false;
        }
    }
}
