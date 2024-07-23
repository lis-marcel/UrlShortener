using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using UrlShortener.Database;
using UrlShortener.Entities;
using UrlShortener.Service.DTO;

namespace UrlShortener.Service
{
    public class SessionService
    {
        private readonly DbStorageContext _context;

        public SessionService(DbStorageContext context)
        {
            _context = context;
        }

        public async Task<Guid?> Login(LoginRequestData loginRequestData)
        {
            var user = await _context.Users.SingleOrDefaultAsync(r => r.Email == loginRequestData.Email);

            if (user is not null && PasswordHashing.VerifyPassword(user.Password, loginRequestData.Password))
            {
                var sessionKey = await CreateSession(user.Id);

                return sessionKey;
            }

            return null;
        }

        public async Task<bool> Logout(string token)
        {
            return await DeleteSession(token);        
        }

        public async Task<bool> RefreshSessionExpiration(string token)
        {
            if (Guid.TryParse(token, out Guid sessionKey))
            {
                var session = await _context.Sessions.SingleOrDefaultAsync(r => r.SessionKey == sessionKey);

                if (session is not null)
                {
                    session.ExpirationTime = DateTime.Now.AddMinutes(20);
                    _context.Sessions.Update(session);
                    await _context.SaveChangesAsync();

                    return true;
                }
            }

            return false;
        }


        private async Task<Guid> CreateSession(Guid userId)
        {
            var session = await _context.Sessions.SingleOrDefaultAsync(r => r.UserId == userId);

            if (session is not null)
            {
                await RefreshSessionExpiration(session.SessionKey.ToString());
            }
            else
            {
                session = new Session(userId);
                _context.Sessions.Add(session);
            }

            await _context.SaveChangesAsync();
            return session.SessionKey;
        }

        private async Task<bool> DeleteSession(string token)
        {
            if (Guid.TryParse(token, out Guid sessionKey))
            {
                var session = await _context.Sessions.SingleOrDefaultAsync(r => r.SessionKey == sessionKey);

                if (session is null)
                {
                    return false;
                }

                _context.Sessions.Remove(session);
                await _context.SaveChangesAsync();

                return true;
            }

            return false;
        }
    }
}
