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

namespace FreeturiloWebApi.Test
{
    public class AppControllerTests
    {

        private static readonly string _serwerPath = @"https://localhost:5001/";
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
            var client = new RestClient(_serwerPath + tokenParameters)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @"{""email"": """ + email + @""", ""password"": """ + password + @""" }";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);

            string token = response.Content[1..^1];

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