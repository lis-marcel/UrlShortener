using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UrlShortener.Database;
using UrlShortener.Service;
using UrlShortener.Entities;

namespace UrlShortener.Service.Test
{
    [TestClass]
    public class CodeGeneratorTest
    {
        private DbContextOptions<DbStorageContext> _options;

        [TestInitialize]
        public void TestInitialize()
        {
            _options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
        }

        [TestMethod]
        public async Task GenerateUniqueCode_ShouldReturnUniqueCode()
        {
            using var context = new DbStorageContext(_options);
            var generator = new CodeGeneratorService(context);

            var uniqueCode = await generator.GenerateUniqueCode();

            Assert.IsNotNull(uniqueCode);
            Assert.AreEqual(7, uniqueCode.Length);
        }

        [TestMethod]
        public async Task GenerateUniqueCode_ShouldReturnDifferentCodes()
        {
            using var context = new DbStorageContext(_options);
            var generator = new CodeGeneratorService(context);

            var codes = new HashSet<string>();

            for (int i = 0; i < 100; i++)
            {
                var code = await generator.GenerateUniqueCode();
                Assert.IsTrue(codes.Add(code), $"Duplicate code generated: {code}");
            }
        }

        [TestMethod]
        public async Task GenerateUniqueCode_ShouldNotReturnExistingCodes()
        {
            using var context = new DbStorageContext(_options);
            var existingCodes = new List<ShortenedUrl>
            {
                new ShortenedUrl { Id = Guid.NewGuid(), Code = "ABC1234" },
                new ShortenedUrl { Id = Guid.NewGuid(), Code = "XYZ5678" }
            };

            context.ShortenedUrls.AddRange(existingCodes);
            await context.SaveChangesAsync();

            var generator = new CodeGeneratorService(context);

            for (int i = 0; i < 100; i++)
            {
                var code = await generator.GenerateUniqueCode();
                Assert.IsFalse(existingCodes.Any(e => e.Code == code), $"Generated code matches existing code: {code}");
            }
        }

        [TestMethod]
        public async Task GenerateUniqueCode_ShouldHandleHighVolume()
        {
            using var context = new DbStorageContext(_options);
            var generator = new CodeGeneratorService(context);

            var codes = new HashSet<string>();

            for (int i = 0; i < 1000; i++)
            {
                var code = await generator.GenerateUniqueCode();
                Assert.IsTrue(codes.Add(code), $"Duplicate code generated: {code}");
            }
        }
    }
}
