using Microsoft.EntityFrameworkCore;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using UrlShortener.Database;
using UrlShortener.Entities;
using UrlShortener.Service.DTOconverters;

namespace UrlShortener.Service
{
    public class UrlShorteningService
    {
        private readonly DbStorageContext _context;
        private static readonly HttpClient httpClient = new() { Timeout = TimeSpan.FromSeconds(5) };

        public UrlShorteningService(DbStorageContext context)
        {
            _context = context;
        }

        public async Task<string> Add(string domain, string url)
        {
            if (!await IsValidUrl(url))
            {
                return "The URL is not valid or unreachable.";
            }

            var code = await GenerateUniqueCode();

            ShortenedUrl shortenedUrls = new()
            {
                Id = Guid.NewGuid(),
                Url = url,
                Code = code,
                ShortUrl = $"{domain}/FoxNet/shortener/{code}",
                CreationTime = DateTime.Now,
            };

            _context.ShortenedUrls.Add(shortenedUrls);
            await _context.SaveChangesAsync();

            return shortenedUrls.Id.ToString();
        }

        public async Task<string?> GetUrlByCode(string code)
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
            var shortenedUrl = _context.ShortenedUrls.SingleOrDefault(r => r.Id == guid);
            if (shortenedUrl == null)
            {
                return false;
            }

            _context.ShortenedUrls.Remove(shortenedUrl);
            _context.SaveChanges();

            return true;
        }

        private async Task<string> GenerateUniqueCode()
        {
            var codeGenerator = new CodeGeneratorService(_context);
            var uniqueCode = await codeGenerator.GenerateUniqueCode();

            return uniqueCode.ToString();
        }

        private static async Task<bool> IsValidUrl(string url)
        {
            if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                return false;
            }

            try
            {
                var request = new HttpRequestMessage(HttpMethod.Head, url);
                var response = await httpClient.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}