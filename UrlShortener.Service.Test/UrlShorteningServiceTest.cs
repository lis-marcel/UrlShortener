using Microsoft.EntityFrameworkCore;
using System;
using UrlShortener.Database;
using Xunit;

namespace UrlShortener.Service.Test
{
    [TestClass]
    public class UrlShorteningServiceTest
    {
        [TestMethod]
        public void AddTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;

            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            service.Add("yt.com", "yt.com/1234");

            Assert.AreEqual(1, context.ShortenedUrls.Count());

            context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void AddReturnTypeTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;

            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var addResultGuid = service.Add("yt.com", "yt.com/1234").Result;

            Assert.AreEqual(typeof(Guid), addResultGuid.GetType());

            context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void DeleteTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;

            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var guid = service.Add("yt.com", "yt.com/1234").Result;

            var deleteResult = service.Delete(guid);

            Assert.AreEqual(0, context.ShortenedUrls.Count());

            context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void DeleteReturnTypeTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;

            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var addResultGuid = service.Add("yt.com", "yt.com/1234").Result;

            var deleteResult = service.Delete(addResultGuid).Result;

            Assert.AreEqual(typeof(bool), deleteResult.GetType());

            context.Database.EnsureDeleted();
        }
    }
}