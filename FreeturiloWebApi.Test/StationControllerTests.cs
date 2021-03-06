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
using AutoMapper;
using Microsoft.Extensions.Hosting;
using FreeturiloWebApi.HttpMethods;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using System;

namespace FreeturiloWebApi.Test
{
    public class StationControllerTests
    {
        private static readonly string serverPath = @"https://localhost:5002/";
        private const string email = "mikolajryll@gmail.com";
        private const string password = "password";

        private readonly IAppMethods appMethods = new AppMethods();
        private readonly IUserMethods userMethods = new UserMethods();
        private readonly IStationMethods stationMethods = new StationMethods();

        private readonly StationDTO[] defaultStations = new StationDTO[]
        {
                new StationDTO() { Id = 1, Bikes = 10, BikeRacks = 15, State = 0, Name = "Trasa Łazienkowska x Marszałkowska", Latitude = 52.296298, Longitude = 20.958358 },
                new StationDTO() { Id = 2, Bikes = 10, BikeRacks = 15, State = 1, Name = "Metro Wierzbno", Latitude = 52.290974, Longitude = 20.929556 },
                new StationDTO() { Id = 3, Bikes = 10, BikeRacks = 15, State = 2, Name = "Dworzec Centralny x Emilii Plater", Latitude = 52.290173, Longitude = 20.95037 }
        };

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
        public void GetAllStations()
        {
            var stations = stationMethods.GetAllStations(serverPath);
            Assert.AreEqual(stations.Length, 8);

            var token = userMethods.Authenticate(serverPath, new() { Email = email, Password = password });

            Assert.Catch<Exception503>(() =>
            {
                appMethods.Stop(serverPath, token);
                stationMethods.GetAllStations(serverPath);
            });

            appMethods.Start(serverPath, token);
        }

        [Test]
        public void GetStation()
        {
            var station = stationMethods.GetStation(serverPath, 1);
            Assert.AreEqual(station.Id, 1);

            Assert.Catch<Exception404>(() =>
            {
                stationMethods.GetStation(serverPath, 123);
            });

            var token = userMethods.Authenticate(serverPath, new() { Email = email, Password = password });

            Assert.Catch<Exception503>(() =>
            {
                appMethods.Stop(serverPath, token);
                stationMethods.GetStation(serverPath, 1);
            });

            appMethods.Start(serverPath, token);
        }

        [Test]
        public void UpdateAllStations()
        {
            var newStations = new StationDTO[]
            {
                new StationDTO() { Id = 1, Bikes = 10, BikeRacks = 20},
                new StationDTO() { Id = 2, Bikes = 15, BikeRacks = 20},
            };
            var token = userMethods.Authenticate(serverPath, new() { Email = email, Password = password });
            stationMethods.UpdateAllStations(serverPath, token, newStations);
            var stations = stationMethods.GetAllStations(serverPath);
            Assert.AreEqual(stations.Length, 8);
            Assert.AreEqual(stations[0].Bikes, 10);
            Assert.AreEqual(stations[1].Bikes, 15);
            Assert.AreEqual(stations[2].Bikes, 10);

            Assert.AreEqual(stations[0].BikeRacks, 20);
            Assert.AreEqual(stations[1].BikeRacks, 20);
            Assert.AreEqual(stations[2].BikeRacks, 15);

            Assert.Catch<Exception400>(() =>
            {
                stationMethods.UpdateAllStations(serverPath, token, null);
            });

            stationMethods.UpdateAllStations(serverPath, token, defaultStations);

            Assert.Catch<Exception401>(() =>
            {
                stationMethods.UpdateAllStations(serverPath, "", Array.Empty<StationDTO>());
            });

            Assert.Catch<Exception404>(() =>
            {
                stationMethods.UpdateAllStations(serverPath, token, new[] { new StationDTO() { Id = 19 } });
            });
        }

        [Test]
        public void ReportStation()
        {
            stationMethods.ReportStation(serverPath, 1);
            var station = stationMethods.GetStation(serverPath, 1);

            Assert.AreEqual(station.State, 1);

            Assert.Catch<Exception404>(() =>
            {
                stationMethods.ReportStation(serverPath, 123);
            });

            var token = userMethods.Authenticate(serverPath, new() { Email = email, Password = password });
            appMethods.Stop(serverPath, token);

            Assert.Catch<Exception503>(() =>
            {
                stationMethods.ReportStation(serverPath, 1);
            });

            appMethods.Start(serverPath, token);
        }

        [Test]
        public void SetStationAsBrokenOrWorking()
        {
            var token = userMethods.Authenticate(serverPath, new() { Email = email, Password = password });
            stationMethods.SetStationAsBroken(serverPath, token, 1);
            var station = stationMethods.GetStation(serverPath, 1);
            Assert.AreEqual(station.State, 2);

            stationMethods.SetStationAsWorking(serverPath, token, 1);
            station = stationMethods.GetStation(serverPath, 1);
            Assert.AreEqual(station.State, 0);

            Assert.Catch<Exception401>(() =>
            {
                stationMethods.SetStationAsBroken(serverPath, "", 1);
            });

            Assert.Catch<Exception401>(() =>
            {
                stationMethods.SetStationAsWorking(serverPath, "", 1);
            });

            Assert.Catch<Exception404>(() =>
            {
                stationMethods.SetStationAsBroken(serverPath, token, 123);
            });

            Assert.Catch<Exception404>(() =>
            {
                stationMethods.SetStationAsWorking(serverPath, token, 123);
            });
        }
    }
}