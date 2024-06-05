using UrlShortener.Database;
using UrlShortener.Entities;

namespace UrlShortener.Service
{
    public class UserService
    {
        private readonly DbStorageContext _context;

        public UserService(DbStorageContext context)
        {
            _context = context;
        }

        public async Task<bool> CreateUser(UserData userData)
        {
            var user = new User()
            {
                Id = Guid.NewGuid(),
                Name = userData.Name,
                Password = PasswordHashing.HashPassword(userData.Password),
                Email = userData.Email,
            };

            _context.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }

        //public async Task<bool> DeleteUser(Guid guid)
        //{
        //    _context.Remove(await _context.Users.SingleAsync(r => r.Id == guid));
        //}
    }
}
