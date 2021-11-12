using NUnit.Framework;
using FreeturiloWebApi.Controllers;
using FreeturiloWebApi.Models;
using FreeturiloWebApi.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Data.Common;
using System.Linq;
using FreeturiloWebApi.Exceptions;
using FreeturiloWebApi.Helpers;
using FreeturiloWebApi.DTO;
using Microsoft.Extensions.Options;

namespace FreeturiloWebApi.Test
{
    public class UserControllerTests
    {
        private UserController _controller;
        private FreeturiloContext _context;
        private void Seed()
        {
            _context.Administrators.AddRange(
                new Administrator() { Id=1, Name="Miko³aj", Surname="Ryll", Email="mikolajryll@gmail.com", NotifyThreshold=1, PasswordHash= "5f4dcc3b5aa765d61d8327deb882cf99" }
                );    
            _context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            var opt = new DbContextOptionsBuilder<FreeturiloContext>().UseInMemoryDatabase(databaseName: "TestsDb").Options;
            _context = new FreeturiloContext(opt);
            Seed();

            var options = Options.Create(new AppSettings { Secret = "very long secret string with very many random characters" });
            var service = new UserService(_context, options);
            _controller = new UserController(service);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
        }

        [Test]
        public void Authenticate()
        {
            var response = _controller.Authenticate(new AuthDTO { Email = "mikolajryll@gmail.com", Password = "password" });
            var result = response.Result as ObjectResult;
            var value = result.Value as string;

            Assert.AreEqual(200, result.StatusCode);
            Assert.IsTrue(value.Length >= 60);

            Assert.Catch<Exception401>(() =>
            {
                var response = _controller.Authenticate(null);
            });

            Assert.Catch<Exception401>(() =>
            {
                var response = _controller.Authenticate(new AuthDTO { Email = "mikolajryll@gmail.com" });
            });

            Assert.Catch<Exception401>(() =>
            {
                var response = _controller.Authenticate(new AuthDTO { Password = "password" });
            });

            Assert.Catch<Exception401>(() =>
            {
                var response = _controller.Authenticate(new AuthDTO());
            });
        }

       
    }
}