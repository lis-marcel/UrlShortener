using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Database;
using UrlShortener.Service;

namespace UrlShortener.Service.Test
{
    [TestClass]
    public class CodeGeneratorTest
    {
        [TestMethod]
        public async Task GenerateUniqeCodeTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var generator = new CodeGeneratorService(context);
            var service = new UrlShorteningService(context);

            // Act
            var objectId = service.Add("https://foxnet.com", "yt.com/1234").Result;
            var selectById = service.GetUrlById(objectId).Result;
            var code = service.FindUrlByCode(selectById.Code).Result;

            var uniqueCode = await generator.GenerateUniqueCode();

            // Assert
            Assert.AreNotEqual(code, uniqueCode);
        }
    }
}
