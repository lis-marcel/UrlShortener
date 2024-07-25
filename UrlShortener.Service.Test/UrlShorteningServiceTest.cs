using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Database;
using UrlShortener.Entities;
using Xunit;

namespace UrlShortener.Service.Test
{
    [TestClass]
    public class UrlShorteningServiceTest
    {
        private DbContextOptions<DbStorageContext> GetInMemoryOptions()
        {
            return new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public async Task Add_ValidUrl_ShouldAddUrl()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var result = await service.Add("https://foxnet.com", "https://example.com");

            Assert.AreEqual(1, context.ShortenedUrls.Count());
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task Add_InvalidUrl_ShouldNotAddUrl()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var result = await service.Add("https://foxnet.com", "invalid-url");

            Assert.AreEqual(0, context.ShortenedUrls.Count());
            Assert.AreEqual("The URL is not valid or unreachable.", result);
        }

        [TestMethod]
        public async Task GetUrlByCode_ValidCode_ShouldReturnUrl()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var addResult = await service.Add("https://foxnet.com", "https://example.com");
            var shortenedUrl = context.ShortenedUrls.First();

            var result = await service.GetUrlByCode(shortenedUrl.Code);

            Assert.AreEqual("https://example.com", result);
        }

        [TestMethod]
        public async Task GetUrlByCode_InvalidCode_ShouldReturnNull()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var result = await service.GetUrlByCode("invalid-code");

            Assert.IsNull(result);
        }

        [TestMethod]
        public async Task Delete_ValidId_ShouldDeleteUrl()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var addResult = await service.Add("https://foxnet.com", "https://example.com");
            var shortenedUrl = context.ShortenedUrls.First();

            var deleteResult = await service.Delete(shortenedUrl.Id);

            Assert.IsTrue(deleteResult);
            Assert.AreEqual(0, context.ShortenedUrls.Count());
        }

        [TestMethod]
        public async Task Delete_InvalidId_ShouldReturnFalse()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var deleteResult = await service.Delete(Guid.NewGuid());

            Assert.IsFalse(deleteResult);
        }

        [TestMethod]
        public async Task GetAllUrls_ShouldReturnAllUrls()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var res1 = await service.Add("https://foxnet.com", "https://guthib.com/");
            var res2 = await service.Add("https://foxnet.com", "https://github.com/");

            var result = await service.GetAllUrls();

            Assert.AreEqual(2, result.Count());
        }

        [TestMethod]
        public async Task GetUrlById_ValidId_ShouldReturnUrlData()
        {
            var options = GetInMemoryOptions();
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var addResult = await service.Add("https://foxnet.com", "https://example.com");
            var shortenedUrl = context.ShortenedUrls.First();

            var result = await service.GetUrlById(shortenedUrl.Id);

            Assert.AreEqual(shortenedUrl.Url, result.Url);
        }
    }
}