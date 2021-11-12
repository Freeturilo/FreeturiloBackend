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
    public class AppControllerTests
    {
        private AppController _controller;
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

            var service = new AppService(_context);
            _controller = new AppController(service);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public void Stop()
        {
            var response = _controller.Stop() as OkResult;
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(AppState.State, AppStateEnum.Stopped);
        }
        [Test]
        public void Start()
        {
            var response = _controller.Start() as OkResult;
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(AppState.State, AppStateEnum.Started);
        }
        [Test]
        public void Demo()
        {
            var response = _controller.Demo() as OkResult;
            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(AppState.State, AppStateEnum.Demo);
        }
        [Test]
        public void SetNotifyTrashold()
        {
            //
        }
    }
}