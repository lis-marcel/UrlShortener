using Microsoft.EntityFrameworkCore;
using ServiceStack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using UrlShortener.Database;
using UrlShortener.Models;
using UrlShortener.Service;
using UrlShortener.Service.DTO;

namespace UrlShortener.Service.Test
{
    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public async Task RegisterUserTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var service = new UserService(context);
            var user = new RegisterRequestData()
            {
                Name = "Test",
                Email = "test@email.com",
                Password = "password",
            };

            // Act
            var res = await service.RegisterUser(user);

            // Assert
            Assert.AreEqual(1, context.Users.Count());

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task UserExistsTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var service = new UserService(context);
            var user = new RegisterRequestData()
            {
                Name = "Test",
                Email = "test@email.com",
                Password = "password",
            };

            // Act
            await service.RegisterUser(user);

            var exists = await service.UserExists(user.Email);

            // Assert
            Assert.IsTrue(exists);
        }

        [TestMethod]
        public async Task LoginTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "AppTestDb.db")
                .Options;
            using var context = new DbStorageContext(options);
            var service = new UserService(context);
            var email = "test@email.com";
            var password = "password";

            // Act
            var user = new RegisterRequestData()
            {
                Name = "Test",
                Email = email,
                Password = password,
            };

            await service.RegisterUser(user);

            var login = new LoginRequestData()
            {
                Email = email,
                Password = password,
            };

            var validateResult = await service.ValidateUser(login);

            // Assert
            Assert.IsTrue(validateResult);

            context.Database.EnsureDeleted();
        }
    }
}
