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
using System;

namespace FreeturiloWebApi.Test
{
    public class RouteControllerTests
    {
        private static readonly string serverPath = @"https://localhost:5005/";
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
        public void GetRouteWhenSystemStopped()
        {
            var token = UserMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });

            AppMethods.Stop(serverPath, token);
            Assert.Catch<Exception503>(() =>
            {
                RouteMethods.GetRoute(serverPath, new RouteParametersDTO());
            });
            AppMethods.Start(serverPath, token);
        }
        [Test]
        public void GetRouteWithNullArgument()
        {
            Assert.Catch<Exception400>(() =>
            {
                RouteMethods.GetRoute(serverPath, null);
            });

            Assert.Catch<Exception400>(() =>
            {
                RouteMethods.GetRoute(serverPath, new RouteParametersDTO()
                {
                    Start = new LocationDTO()
                });
            });

            Assert.Catch<Exception400>(() =>
            {
                RouteMethods.GetRoute(serverPath, new RouteParametersDTO()
                {
                    End = new LocationDTO()
                });
            });
        }
        [Test]
        public void GetRouteForLocationOutOfWarsaw()
        {
            Assert.Catch<Exception404>(() =>
            {
                var parameters = new RouteParametersDTO
                {
                    Criterion = 0,
                    Start = new LocationDTO()
                    {
                        Latitude = 10.0,
                        Longitude = 10.0,
                    },
                    End = new LocationDTO()
                    {
                        Latitude = 50.0,
                        Longitude = 10.0,
                    }
                };
                RouteMethods.GetRoute(serverPath, parameters);
            });
        }
        [Test]
        public void GetRouteForIncorrectCriteria()
        {
            Assert.Catch<Exception400>(() =>
            {
                var parameters = new RouteParametersDTO
                {
                    Criterion = 10,
                    Start = new LocationDTO()
                    {
                        Latitude = 52.1,
                        Longitude = 21.0,
                    },
                    End = new LocationDTO()
                    {
                        Latitude = 52.1,
                        Longitude = 21.0,
                    }
                };
                RouteMethods.GetRoute(serverPath, parameters);
            });
        }
    }
}