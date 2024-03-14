using Microsoft.EntityFrameworkCore;
using System.Collections;
using UrlShortener.Database;
using UrlShortener.Entities;

namespace UrlShortener.Service
{
    public class UrlShorteningService
    {
        private readonly Random _random = new();
        private readonly DbStorageContext _context;
        private const int CodeLength = 7;
        private const string Alphabet =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        public UrlShorteningService(DbStorageContext context)
        {
            _context = context;
        }

        public async Task<Guid> Add(string domain, string url)
        {
            var code = await GenerateUniqueCode();

            ShortenedUrls shortenedUrls = new()
            {
                Id = new Guid(),
                Url = url,
                Code = code,
                ShortUrl = $"{domain}/{code}",
                CreationTime = DateTime.Now,
            };

            _context.Add(shortenedUrls);
            await _context.SaveChangesAsync();
            _context.Dispose();

            return shortenedUrls.Id;
        }
        public async Task<string> FindUrl(string code)
        {
            var shortenedUrl = await _context
                .ShortenedUrls
                .FirstOrDefaultAsync(r => r.Code == code);

            if (shortenedUrl is null)
            {
                return null;
            }

            return shortenedUrl.Url;
        }
        public async Task<IEnumerable<ShortenedUrls>> GetAllUrls()
        {
            return await _context.ShortenedUrls.Select(r => new ShortenedUrls(r)).ToArray();
        }
        public async Task<bool> Delete(Guid guid)
        {
            var shortenedUrl = _context.ShortenedUrls.Single(r => r.Id == guid);

            _context.ShortenedUrls.Remove(shortenedUrl);
            var result = await _context.SaveChangesAsync();

            return true ? result > 0 : false;
        }
        private async Task<string> GenerateUniqueCode()
        {
            var codeChars = new char[CodeLength];
            int maxValue = Alphabet.Length;

            for (var i = 0; i < CodeLength; i++)
            {
                int randomIndex = _random.Next(maxValue);

                codeChars[i] = Alphabet[randomIndex];
            }

            string stringCode = new(codeChars);

            if (await CodeExists(stringCode))
            {
                await GenerateUniqueCode();
            }

            return new string(codeChars);
        }
        private async Task<bool> CodeExists(string code)
        {
            bool doesExist = await _context.ShortenedUrls.AnyAsync(r => r.Code == code);

            return true ? doesExist : false;
        }
    }
}