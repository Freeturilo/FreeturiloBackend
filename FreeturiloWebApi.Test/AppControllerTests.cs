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
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using RestSharp;
using FreeturiloWebApi.HttpMethods;

namespace FreeturiloWebApi.Test
{
    public class AppControllerTests
    {

        private static readonly string serverPath = @"https://localhost:5001/";
        private const string email = "mikolajryll@gmail.com";
        private const string password = "password";
        private const string tokenParameters = "user";

        private IHost host;

        [OneTimeSetUp]
        public void Setup()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                })
                .Build();

            host.Start();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            host.StopAsync().Wait();
            host.Dispose();
        }
        [Test]
        public void Stop()
        {
            var result = AppMethods.Stop(serverPath, "");
            Assert.AreEqual(result, false);

            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            result = AppMethods.Stop(serverPath, token);
            Assert.AreEqual(result, true);
        }
        [Test]
        public void Start()
        {
            var result = AppMethods.Start(serverPath, "");
            Assert.AreEqual(result, false);

            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            result = AppMethods.Start(serverPath, token);
            Assert.AreEqual(result, true);
        }
        [Test]
        public void Demo()
        {
            var result = AppMethods.Demo(serverPath, "");
            Assert.AreEqual(result, false);

            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            result = AppMethods.Demo(serverPath, token);
            Assert.AreEqual(result, true);
        }
        [Test]
        public void SetNotifyTrashold()
        {
            //
        }
    }
}