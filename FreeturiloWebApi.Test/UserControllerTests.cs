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

namespace FreeturiloWebApi.Test
{
    public class UserControllerTests
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
        public void CorrectAuthenticate()
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

            Assert.IsTrue(token.Length > 40);
        }

        [Test]
        public void IncorrectAuthenticate()
        {
            var client = new RestClient(_serwerPath + tokenParameters)
            {
                Timeout = -1
            };
            var request = new RestRequest(Method.POST);
            request.AddHeader("Content-Type", "application/json");
            var body = @" ""password"": """ + password + @""" }";
            request.AddParameter("application/json", body, ParameterType.RequestBody);
            IRestResponse response = client.Execute(request);
            string token = response.Content[1..^1];

            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}