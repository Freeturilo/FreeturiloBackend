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

namespace FreeturiloWebApi.Test
{
    public class StationControllerTests
    {
        private static readonly string serverPath = @"https://localhost:5002/";
        private const string email = "mikolajryll@gmail.com";
        private const string password = "password";

        private readonly StationDTO[] defaultStations = new StationDTO[]
        {
                new StationDTO() { Id = 1, Bikes = 10, BikeRacks = 15, State = 0, Name = "Trasa £azienkowska x Marsza³kowska", Latitude = 52.296298, Longitude = 20.958358 },
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
            var stations = StationMethods.GetAllStations(serverPath);
            Assert.AreEqual(stations.Length, 3);

            var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });

            Assert.Catch<Exception503>(() =>
            {
                AppMethods.Stop(serverPath, token);
                StationMethods.GetAllStations(serverPath);
            });

            AppMethods.Start(serverPath, token);
        }

        [Test]
        public void GetStation()
        {
            var station = StationMethods.GetStation(serverPath, 1);
            Assert.AreEqual(station.Id, 1);

            Assert.Catch<Exception404>(() =>
            {
                StationMethods.GetStation(serverPath, 123);
            });

            var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });

            Assert.Catch<Exception503>(() =>
            {
                AppMethods.Stop(serverPath, token);
                StationMethods.GetStation(serverPath, 1);
            });

            AppMethods.Start(serverPath, token);
        }

        [Test]
        public void AddNewStation()
        {
            var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });
            var newStation = new StationDTO() { Id = 4, Bikes = 3, Latitude = 52.285914, Longitude = 20.940561 };
            StationMethods.AddNewStation(serverPath, token, newStation);
            var stations = StationMethods.GetAllStations(serverPath);
            Assert.AreEqual(stations.Length, 4);

            Assert.Catch<Exception400>(() =>
            {
                var newStation = new StationDTO() { Id = 1, Bikes = 3 };
                StationMethods.AddNewStation(serverPath, token, newStation);
            });

            Assert.Catch<Exception400>(() =>
            {
                StationMethods.AddNewStation(serverPath, token, null);
            });

            StationMethods.UpdateAllStations(serverPath, token, defaultStations);

            Assert.Catch<Exception401>(() =>
            {
                StationMethods.AddNewStation(serverPath, "", newStation);
            });
        }

        [Test]
        public void UpdateAllStations()
        {
            var newStations = new StationDTO[]
            {
                new StationDTO() {Id = 6},
            };

            var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });
            StationMethods.UpdateAllStations(serverPath, token, newStations);
            var stations = StationMethods.GetAllStations(serverPath);
            Assert.AreEqual(stations.Length, 1);
            Assert.AreEqual(stations[0].Id, 6);

            Assert.Catch<Exception400>(() =>
            {
                StationMethods.UpdateAllStations(serverPath, token, null);
            });

            StationMethods.UpdateAllStations(serverPath, token, defaultStations);

            Assert.Catch<Exception401>(() =>
            {
                StationMethods.UpdateAllStations(serverPath, "", newStations);
            });
        }

        [Test]
        public void UpdateStation()
        {
            var newStation = new StationDTO() { Id = 1, Bikes = 12, Latitude = 52.285914, Longitude = 20.940561 };
            var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });

            StationMethods.UpdateStation(serverPath, token, 1, newStation);
            var station = StationMethods.GetStation(serverPath, 1);

            Assert.AreEqual(station.Bikes, 12);

            Assert.Catch<Exception404>(() =>
            {
                StationMethods.UpdateStation(serverPath, token, 123, newStation);
            });

            Assert.Catch<Exception400>(() =>
            {
                StationMethods.UpdateStation(serverPath, token, 1, null);
            });

            Assert.Catch<Exception401>(() =>
            {
                StationMethods.UpdateStation(serverPath, "", 1, newStation);
            });
        }

        [Test]
        public void ReportStation()
        {
            StationMethods.ReportStation(serverPath, 1);
            var station = StationMethods.GetStation(serverPath, 1);

            Assert.AreEqual(station.State, 1);

            Assert.Catch<Exception404>(() =>
            {
                StationMethods.ReportStation(serverPath, 123);
            });

            var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });
            AppMethods.Stop(serverPath, token);

            Assert.Catch<Exception503>(() =>
            {
                StationMethods.ReportStation(serverPath, 1);
            });

            AppMethods.Start(serverPath, token);
        }

        [Test]
        public void SetStationAsBrokenOrWorking()
        {
            var token = UserMethods.Authenticate(serverPath, new() { Email = email, Password = password });
            StationMethods.SetStationAsBroken(serverPath, token, 1);
            var station = StationMethods.GetStation(serverPath, 1);
            Assert.AreEqual(station.State, 2);

            StationMethods.SetStationAsWorking(serverPath, token, 1);
            station = StationMethods.GetStation(serverPath, 1);
            Assert.AreEqual(station.State, 0);

            Assert.Catch<Exception401>(() =>
            {
                StationMethods.SetStationAsBroken(serverPath, "", 1);
            });

            Assert.Catch<Exception401>(() =>
            {
                StationMethods.SetStationAsWorking(serverPath, "", 1);
            });

            Assert.Catch<Exception404>(() =>
            {
                StationMethods.SetStationAsBroken(serverPath, token, 123);
            });

            Assert.Catch<Exception404>(() =>
            {
                StationMethods.SetStationAsWorking(serverPath, token, 123);
            });
        }
    }
}