using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Entities;
using UrlShortener.Service.DTOconverters;

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

            ShortenedUrl shortenedUrls = new()
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
        public async Task<IEnumerable<ShortenedUrlData>> GetAllUrls()
        {
            return _context.ShortenedUrls.Select
                (r => ConvertShortenedUrl.ShortendedUrlToShortendedUrlData(r))
                .ToList();
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
            string stringCode;
            do
            {
                var codeChars = new char[CodeLength];
                int maxValue = Alphabet.Length;

                for (var i = 0; i < CodeLength; i++)
                {
                    int randomIndex = _random.Next(maxValue);
                    codeChars[i] = Alphabet[randomIndex];
                }

                stringCode = new string(codeChars);
            }
            while (await CodeExists(stringCode));

            return stringCode;
        }
        private async Task<bool> CodeExists(string code)
        {
            bool doesExist = await _context.ShortenedUrls.AnyAsync(r => r.Code == code);
            return doesExist;
        }
    }
}