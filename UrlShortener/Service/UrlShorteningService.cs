using Microsoft.EntityFrameworkCore;
using UrlShortener.Database;
using UrlShortener.Entities;
using UrlShortener.Service.DTOconverters;

namespace UrlShortener.Service
{
    public class UrlShorteningService
    {
        private readonly DbStorageContext _context;

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

            _context.ShortenedUrls.Add(shortenedUrls);
            await _context.SaveChangesAsync();

            return shortenedUrls.Id;
        }
        public async Task<string?> FindUrlByCode(string code)
        {
            var shortenedUrl = await _context
                .ShortenedUrls
                .FirstOrDefaultAsync(r => r.Code == code);

            return shortenedUrl?.Url;
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
            var codeGenerator = new CodeGeneratorService(_context);
            var uniqueCode = await codeGenerator.GenerateUniqueCode();

            return uniqueCode.ToString();
        }
    }
}