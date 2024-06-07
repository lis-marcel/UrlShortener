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
        public async Task CreateSessionTest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<DbStorageContext>()
                .UseInMemoryDatabase(databaseName: "CreateSessionTest")
                .Options;
            using var context = new DbStorageContext(options);
            var userService = new UserService(context);
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
            var result = await userService.Login(loginData);

            // Assert
            Assert.IsNotNull(result);

            context.Database.EnsureDeleted();
        }
    }
}
