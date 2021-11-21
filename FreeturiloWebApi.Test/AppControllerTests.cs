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
        public void Stop()
        {
            Assert.Catch<Exception401>(() =>
            {
                AppMethods.Stop(serverPath, "");
            });

            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            AppMethods.Stop(serverPath, token);
            Assert.AreEqual(AppState.State, AppStateEnum.Stopped);
            AppMethods.Start(serverPath, token);
        }
        [Test]
        public void Start()
        {
            Assert.Catch<Exception401>(() =>
            {
                AppMethods.Start(serverPath, "");
            });

            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            AppMethods.Start(serverPath, token);
            Assert.AreEqual(AppState.State, AppStateEnum.Started);

        }
        [Test]
        public void Demo()
        {
            Assert.Catch<Exception401>(() =>
            {
                AppMethods.Demo(serverPath, "");
            });

            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            AppMethods.Demo(serverPath, token);
            Assert.AreEqual(AppState.State, AppStateEnum.Demo);

            AppMethods.Start(serverPath, token);
        }
        [Test]
        public void Status()
        {
            Assert.Catch<Exception401>(() =>
            {
                AppMethods.Status(serverPath, "");
            });

            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            
            AppMethods.Demo(serverPath, token);
            var state = AppMethods.Status(serverPath, token);
            Assert.AreEqual(state, 2);

            AppMethods.Start(serverPath, token);
        }
        [Test]
        public void SetNotifyTrashold()
        {
            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            AppMethods.SetReportTrashold(serverPath, token, 7);
            AppMethods.SetReportTrashold(serverPath, token, 3);

            StationMethods.ReportStation(serverPath, 1);
            StationMethods.ReportStation(serverPath, 1);

            AppMethods.SetReportTrashold(serverPath, token, 1);

            Assert.Catch<Exception401>(() =>
            {
                AppMethods.SetReportTrashold(serverPath, "", 20);
            });
        }
    }
}