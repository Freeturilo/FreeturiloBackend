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
using System.Configuration;
using Microsoft.AspNetCore.Hosting;
using RestSharp;
using System.Net;
using FreeturiloWebApi.HttpMethods;

namespace FreeturiloWebApi.Test
{
    public class UserControllerTests
    {
        private static readonly string serverPath = @"https://localhost:5003/";
        private const string email = "mikolajryll@gmail.com";
        private const string password = "password";

        private IHost host; 

        [OneTimeSetUp]
        public void Setup()
        {
            host = Host.CreateDefaultBuilder()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<TestStartup>();
                    webBuilder.UseUrls(serverPath);
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
        public void CorrectAuthenticate()
        {
            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            Assert.IsTrue(token.Length > 40);
        }

        [Test]
        public void IncorrectAuthenticate()
        {
            Assert.Catch<Exception401>(() =>
            {
                UserMethods.Authenticate(serverPath, new AuthDTO { Email = "incorrect", Password = "made up" });
            });

            Assert.Catch<Exception401>(() =>
            {
                UserMethods.Authenticate(serverPath, new AuthDTO { Password = password });
            });

            Assert.Catch<Exception401>(() =>
            {
                UserMethods.Authenticate(serverPath, new AuthDTO { Email = email });
            });

            Assert.Catch<Exception401>(() =>
            {
                UserMethods.Authenticate(serverPath, new AuthDTO());
            });

            Assert.Catch<Exception401>(() =>
            {
                UserMethods.Authenticate(serverPath, null);
            });
        }
    }
}