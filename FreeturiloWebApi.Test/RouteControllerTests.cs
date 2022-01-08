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

        private readonly IAppMethods appMethods = new AppMethods();
        private readonly IUserMethods userMethods = new UserMethods();
        private readonly IRouteMethods routeMethods = new RouteMethods();

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
            var token = userMethods.Authenticate(serverPath, new AuthDTO { Email = email, Password = password });

            appMethods.Stop(serverPath, token);
            Assert.Catch<Exception503>(() =>
            {
                routeMethods.GetRoute(serverPath, new RouteParametersDTO());
            });
            appMethods.Start(serverPath, token);
        }
        [Test]
        public void GetRouteWithNullArgument()
        {
            Assert.Catch<Exception400>(() =>
            {
                routeMethods.GetRoute(serverPath, null);
            });

            Assert.Catch<Exception400>(() =>
            {
                routeMethods.GetRoute(serverPath, new RouteParametersDTO()
                {
                    Start = new LocationDTO()
                });
            });

            Assert.Catch<Exception400>(() =>
            {
                routeMethods.GetRoute(serverPath, new RouteParametersDTO()
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
                        Latitude = 50.0,
                        Longitude = 10.0,
                    },
                    End = new LocationDTO()
                    {
                        Latitude = 50.0,
                        Longitude = 10.0,
                    }
                };
                routeMethods.GetRoute(serverPath, parameters);
            });

            Assert.Catch<Exception404>(() =>
            {
                var parameters = new RouteParametersDTO
                {
                    Criterion = 0,
                    Start = new LocationDTO()
                    {
                        Latitude = 59.1,
                        Longitude = 10.0,
                    },
                    End = new LocationDTO()
                    {
                        Latitude = 52.1,
                        Longitude = 10.0,
                    }
                };
                routeMethods.GetRoute(serverPath, parameters);
            });

            Assert.Catch<Exception404>(() =>
            {
                var parameters = new RouteParametersDTO
                {
                    Criterion = 0,
                    Start = new LocationDTO()
                    {
                        Latitude = 52.1,
                        Longitude = 10.0,
                    },
                    End = new LocationDTO()
                    {
                        Latitude = 52.1,
                        Longitude = 10.0,
                    }
                };
                routeMethods.GetRoute(serverPath, parameters);
            });

            Assert.Catch<Exception404>(() =>
            {
                var parameters = new RouteParametersDTO
                {
                    Criterion = 0,
                    Start = new LocationDTO()
                    {
                        Latitude = 52.1,
                        Longitude = 100.0,
                    },
                    End = new LocationDTO()
                    {
                        Latitude = 52.1,
                        Longitude = 100.0,
                    }
                };
                routeMethods.GetRoute(serverPath, parameters);
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
                routeMethods.GetRoute(serverPath, parameters);
            });
        }

        [Test]
        public void GetCheapestRoute()
        {
            var parameters = new RouteParametersDTO
            {
                Criterion = 0,
                Start = new LocationDTO()
                {
                    Longitude = 20.97481,
                    Latitude = 52.220877,
                },
                End = new LocationDTO()
                {
                    Longitude = 21.043405,
                    Latitude = 52.249512,
                }
            };
            var r1 = routeMethods.GetRoute(serverPath, parameters);
            Assert.IsTrue(r1[0].Cost == 0);
            parameters.Stops = new LocationDTO[] {
                new LocationDTO()
                {
                    Longitude = 21.043405,
                    Latitude = 52.249512,
                },
                new LocationDTO()
                {
                    Longitude = 20.97481,
                    Latitude = 52.220877,
                },
            };
            var r2 = routeMethods.GetRoute(serverPath, parameters);
            Assert.IsTrue(r2[0].Cost == 0);

            parameters.Stops = new LocationDTO[] {
                new LocationDTO()
                {
                    Longitude = 21.043405,
                    Latitude = 52.220877,
                },
            };
            var r3 = routeMethods.GetRoute(serverPath, parameters);
            Assert.IsTrue(r3[0].Cost == 0);

        }

        [Test]
        public void GetFastestRoute()
        {
            var parameters = new RouteParametersDTO
            {
                Criterion = 1,
                Start = new LocationDTO()
                {
                    Longitude = 20.97481,
                    Latitude = 52.220877,
                },
                End = new LocationDTO()
                {
                    Longitude = 20.97485,
                    Latitude = 52.220889,
                }
            };
            var r1 = routeMethods.GetRoute(serverPath, parameters);
            Assert.IsTrue(r1[0].Time <= 20 * 60);
            parameters = new RouteParametersDTO
            {
                Criterion = 1,
                Start = new LocationDTO()
                {
                    Longitude = 20.97481,
                    Latitude = 52.220877,
                },
                End = new LocationDTO()
                {
                    Longitude = 21.043405,
                    Latitude = 52.249512,
                }
            };
            var r2 = routeMethods.GetRoute(serverPath, parameters);
            Assert.IsTrue(r2[0].Time <= 20 * 60);
            parameters.Stops = new LocationDTO[] {
                new LocationDTO()
                {
                    Longitude = 21.043405,
                    Latitude = 52.249512,
                },
                new LocationDTO()
                {
                    Longitude = 20.97481,
                    Latitude = 52.220877,
                },
            };
            var r3 = routeMethods.GetRoute(serverPath, parameters);
            Assert.IsTrue(r3[0].Time <= 20 * 60);
        }

        [Test]
        public void GetOptimalRoute()
        {
            var parameters = new RouteParametersDTO
            {
                Criterion = 1,
                Start = new LocationDTO()
                {
                    Longitude = 20.97481,
                    Latitude = 52.220877,
                },
                End = new LocationDTO()
                {
                    Longitude = 20.97485,
                    Latitude = 52.220889,
                }
            };
            var r1_fast = routeMethods.GetRoute(serverPath, parameters);
            parameters.Criterion = 2;
            var r1_opt = routeMethods.GetRoute(serverPath, parameters);
            parameters.Criterion = 0;
            var r1_cheap = routeMethods.GetRoute(serverPath, parameters);

            var (t1_fast, t1_opt_, t1_cheap) = (r1_fast.Sum(e => e.Time), r1_opt.Sum(e => e.Time), r1_cheap.Sum(e => e.Time));
            var (c1_fast, c1_opt_, c1_cheap) = (r1_fast.Sum(e => e.Cost), r1_opt.Sum(e => e.Cost), r1_cheap.Sum(e => e.Cost));

            Assert.IsTrue(t1_fast <= t1_opt_ && t1_opt_ <= t1_cheap);
            Assert.IsTrue(c1_fast >= c1_opt_ && c1_opt_ >= c1_cheap);
        }
    }
}