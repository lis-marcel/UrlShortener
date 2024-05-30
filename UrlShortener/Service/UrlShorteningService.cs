using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Entities;
using UrlShortener.Service.DTOconverters;
using System.Linq;

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
                Id = Guid.NewGuid(),
                Url = url,
                Code = code,
                ShortUrl = $"{domain}/{code}",
                CreationTime = DateTime.Now,
            };

            _context.Add(shortenedUrls);
            await _context.SaveChangesAsync();

            return shortenedUrls.Id;
        }
        public async Task<string> FindUrlByCode(string code)
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
        public async Task<ShortenedUrlData> GetUrlById(Guid guid)
        {
            var url = await _context.ShortenedUrls.SingleAsync(r => r.Id == guid);

            var urlData = new ShortenedUrlData()
            {
                Id = url.Id,
                Url = url.Url,
                Code = url.Code,
                ShortUrl = url.ShortUrl,
                CreationTime = url.CreationTime,
            };

            return urlData;
        }
        public async Task<IEnumerable<ShortenedUrlData>> GetAllUrls()
        {
            return await _context.ShortenedUrls.Select
                (r => ConvertShortenedUrl.ShortendedUrlToShortendedUrlData(r))
                .ToListAsync();
        }
        public async Task<bool> Delete(Guid guid)
        {
            var shortenedUrl = _context.ShortenedUrls.Single(r => r.Id == guid);

            _context.ShortenedUrls.Remove(shortenedUrl);
            var result = await _context.SaveChangesAsync();

            return result > 0;
        }
        public async Task<string> GenerateUniqueCode()
        {
            string stringCode;
            var existingCodesList = await _context.ShortenedUrls.Select(r => r.Code).ToListAsync();
            var existingCodes = new HashSet<string>(existingCodesList);

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
            while (existingCodes.Contains(stringCode));

            return stringCode;
        }
        public async Task<bool> CodeExist(string code)
        {
            bool exist = await _context.ShortenedUrls.AnyAsync(r => r.Code == code);
            return exist;
        }
    }
}