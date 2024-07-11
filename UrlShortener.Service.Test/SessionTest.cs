using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UrlShortener.Database;
using UrlShortener.Entities;
using UrlShortener.Service.DTO;

namespace UrlShortener.Service.Test
{
    [TestClass]
    public class SessionTest
    {
        [TestMethod]
        public async Task LoginTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "LoginTest")
                .Options;
            using var context = new DbStorageContext(options);
            var userService = new UserService(context);
            var sessionService = new SessionService(context);
            var email = "test@mail.com";
            var password = "test";
            var user = new RegisterRequestData()
            {
                Name = "Test",
                Password = password,
                Email = email,
            };
            var loginData = new LoginRequestData()
            {
                Email = email,
                Password = password
            };
            // Act
            await userService.RegisterUser(user);
            var result = await sessionService.Login(loginData);

            // Assert
            Assert.IsInstanceOfType<Guid>(result);

            context.Database.EnsureDeleted();
        }

        [TestMethod]
        public async Task LogoutTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "LoginTest")
                .Options;
            using var context = new DbStorageContext(options);
            var userService = new UserService(context);
            var sessionService = new SessionService(context);

            var email = "test@mail.com";
            var password = "test";
            var user = new RegisterRequestData()
            {
                Name = "Test",
                Password = password,
                Email = email,
            };
            var loginData = new LoginRequestData()
            {
                Email = email,
                Password = password
            };

            await userService.RegisterUser(user);
            var result = await sessionService.Login(loginData);

            // Act
            var logoutResult = await sessionService.Logout(result.ToString());

            // Assert
            Assert.IsTrue(logoutResult);
        }
    }
}
