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

        private static readonly string serverPath = @"https://localhost:5006/";
        private const string email = "mikolajryll@gmail.com";
        private const string password = "password";
        private readonly IAppMethods appMethods = new AppMethods();
        private readonly IUserMethods userMethods = new UserMethods();
        private readonly IStationMethods stationMethods = new StationMethods();

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
                appMethods.Stop(serverPath, "");
            });

            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            appMethods.Stop(serverPath, token);
            var state = appMethods.Status(serverPath, token);
            Assert.AreEqual(state, 2);
            appMethods.Start(serverPath, token);
        }
        [Test]
        public void InvalidSetStatus()
        {
            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            var client = new RestClient(serverPath + "app/state/7")
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            request.AddHeader("api-key", token);
            var res = client.Execute(request);

            Assert.AreEqual(res.StatusCode, System.Net.HttpStatusCode.BadRequest);
        }
        [Test]
        public void Start()
        {
            Assert.Catch<Exception401>(() =>
            {
                appMethods.Start(serverPath, "");
            });

            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            appMethods.Start(serverPath, token);
            var state = appMethods.Status(serverPath, token);
            Assert.AreEqual(state, 0);
        }
        [Test]
        public void Demo()
        {
            Assert.Catch<Exception401>(() =>
            {
                appMethods.Demo(serverPath, "");
            });

            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            appMethods.Demo(serverPath, token);
            var state = appMethods.Status(serverPath, token);
            Assert.AreEqual(state, 1);
            appMethods.Start(serverPath, token);
        }
        [Test]
        public void Status()
        {
            Assert.Catch<Exception401>(() =>
            {
                appMethods.Status(serverPath, "");
            });

            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            
            appMethods.Demo(serverPath, token);
            var state = appMethods.Status(serverPath, token);
            Assert.AreEqual(state, 1);

            appMethods.Start(serverPath, token);
        }
        [Test]
        public void SetNotifyThreshold()
        {
            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            appMethods.SetReportThreshold(serverPath, token, 7);
            appMethods.SetReportThreshold(serverPath, token, 3);

            stationMethods.ReportStation(serverPath, 1);
            stationMethods.ReportStation(serverPath, 1);

            appMethods.SetReportThreshold(serverPath, token, 1);
            var threshold = appMethods.GetReportThreshold(serverPath, token);

            Assert.AreEqual(threshold, 1);


            Assert.Catch<Exception401>(() =>
            {
                appMethods.SetReportThreshold(serverPath, "", 20);
            });
        }
        [Test]
        public void GetNotifyThreshold()
        {
            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });
            var threshold = appMethods.GetReportThreshold(serverPath, token);
            Assert.AreEqual(1, threshold);

            Assert.Catch<Exception401>(() =>
            {
                appMethods.GetReportThreshold(serverPath, "");
            });
        }
    }
}