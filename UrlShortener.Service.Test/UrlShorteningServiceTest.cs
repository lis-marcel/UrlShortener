using Microsoft.EntityFrameworkCore;
using System;
using UrlShortener.Database;
using UrlShortener.Entities;
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

            service.Add("https://foxnet.com", "yt.com/1234");

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

            var addResultGuid = service.Add("https://foxnet.com", "yt.com/1234").Result;

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

            var guid = service.Add("https://foxnet.com", "yt.com/1234").Result;

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

            var addResultGuid = service.Add("https://foxnet.com", "yt.com/1234").Result;

            var deleteResult = service.Delete(addResultGuid).Result;

            Assert.AreEqual(typeof(bool), deleteResult.GetType());

            context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void GetUrlByIdTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var expectedUrl = "yt.com/10";
            var expectedDomain = "https://foxnet.com";
            var addResultGuid = service.Add(expectedDomain, expectedUrl).Result;

            var res = service.GetUrlById(addResultGuid).Result;

            Assert.AreEqual(expectedUrl, res.Url);

            context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void GetAllUrlsTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            service.Add("yt.com", "yt.com/10");
            service.Add("yt.com", "yt.com/200");
            service.Add("yt.com", "yt.com/3000");

            var res = service.GetAllUrls().Result;

            Assert.AreEqual(3, res.Count());

            context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void FindUrlByCodeTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var addResGuid =  service.Add("yt.com/10", "https://foxnet.com").Result;
            var foundObjectByGuid = service.GetUrlById(addResGuid).Result;
            var foundObjectByCode = service.FindUrlByCode(foundObjectByGuid.Code).Result;

            Assert.AreEqual(foundObjectByGuid.Url, foundObjectByCode);           

            context.Database.EnsureDeleted();
        }
        [TestMethod]
        public void CodeExistTest()
        {
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var service = new UrlShorteningService(context);

            var addResGuid = service.Add("yt.com/10", "https://foxnet.com").Result;
            var foundObjectByGuid = service.GetUrlById(addResGuid).Result;

            var code = foundObjectByGuid.Code;

            var codeExist = service.CodeExist(code).Result;

            Assert.IsTrue(codeExist);
        }
    }
}