using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Entities;

namespace UrlShortener.Service
{
    public class SessionService
    {
        private readonly DbStorageContext _context;

        public SessionService(DbStorageContext context)
        {
            _context = context;
        }

        public async Task<Guid> CreateSession(string email)
        {
            var session = await _context.Sessions.SingleOrDefaultAsync(r => r.Email == email);

            if (session is null)
            {
                session = new Session(email);
                _context.Sessions.Add(session);
            }
            else
            {
                session.SessionKey = Guid.NewGuid();
                session.CreationTime = DateTime.Now;
                _context.Sessions.Update(session);
            }

            await _context.SaveChangesAsync();

            return session.SessionKey;
        }

        public async Task<bool> DeleteSession(string email)
        {
            var session = await _context.Sessions.SingleOrDefaultAsync(r => r.Email == email);

            if (session is null)
            {
                return false;
            }

            _context.Sessions.Remove(session);
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
