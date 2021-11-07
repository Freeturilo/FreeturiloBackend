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

namespace FreeturiloWebApi.Test
{
    public class StationControllerTests
    {
        private StationController _controller;
        private FreeturiloContext _context;
        private int _testNo;
        private void Seed()
        {
            _context.Administrators.AddRange(
                new Administrator() { Id=1, Name="Miko�aj", Surname="Ryll", Email="mikolajryll@gmail.com", NotifyThreshold=1, PasswordHash="hash" }
                );

            _context.Stations.AddRange(
                new Station() { Id = 1, AvailableBikes=10, AvailableRacks=15, State=0, Name="Trasa �azienkowska x Marsza�kowska", Reports=0, Lat=52, Lon=48 },
                new Station() { Id = 2, AvailableBikes=10, AvailableRacks=15, State=1, Name="Metro Wierzbno", Reports=1, Lat=52, Lon=48 },
                new Station() { Id = 3, AvailableBikes=10, AvailableRacks=15, State=2, Name="Dworzec Centralny x Emilii Plater", Reports=10, Lat=52, Lon=48 }
                );

            _context.Routes.AddRange(
                new Route() { Id=1, Cost=10, RouteJSON="", StartId=1, StopId=2, Time=12},
                new Route() { Id=2, Cost=10, RouteJSON="", StartId=1, StopId=3, Time=12},
                new Route() { Id=3, Cost=10, RouteJSON="", StartId=2, StopId=1, Time=12},
                new Route() { Id=4, Cost=10, RouteJSON="", StartId=2, StopId=3, Time=12},
                new Route() { Id=5, Cost=10, RouteJSON="", StartId=3, StopId=1, Time=12},
                new Route() { Id=6, Cost=10, RouteJSON="", StartId=3, StopId=2, Time=12}
                );
            
            _context.SaveChanges();
        }

        [SetUp]
        public void Setup()
        {
            var opt = new DbContextOptionsBuilder<FreeturiloContext>().UseInMemoryDatabase(databaseName: "StationTestsDb"+(_testNo++)).Options;
            _context = new FreeturiloContext(opt);
            Seed();

            var config = new MapperConfiguration(cfg => cfg.AddProfile<AutoMapperProfile>());
            var _mapper = config.CreateMapper();
            var service = new StationService(_context, _mapper);
            _controller = new StationController(service);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

        [Test]
        public void GetAllStations()
        {
            var response = _controller.GetAllStations();
            var result = response.Result as ObjectResult;
            var value = result.Value as StationDTO[];

            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(3, value.Length);
        }

        [Test]
        public void GetStation()
        {
            var response = _controller.GetStation(1);
            var result = response.Result as ObjectResult;
            var value = result.Value as StationDTO;

            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(1, value.Id);

            Assert.Catch<Exception404>(() =>
            {
                response = _controller.GetStation(10);
            });
        }

        [Test]
        public void AddNewStation()
        {
            var response = _controller.AddNewStation(new StationDTO(){Id=4});
            var result = response.Result as ObjectResult;
            var value = result.Value as StationDTO;

            Assert.AreEqual(200, result.StatusCode);
            Assert.AreEqual(4, value.Id);
            Assert.AreEqual(4, _context.Stations.Count());

            Assert.Catch<Exception400>(() =>
            {
                response = _controller.AddNewStation(new StationDTO(){Id = 1});
            });
        }

        [Test]
        public void UpdateAllStations()
        {
            var newStations = new StationDTO[]
            {
                new StationDTO() {Id = 6},
                new StationDTO() {Id = 7},
                new StationDTO() {Id = 8},
            };

            var response = _controller.UpdateAllStations(newStations) as OkResult;

            Assert.AreEqual(200, response.StatusCode);
            Assert.IsTrue(newStations.All(ns => _context.Stations.Where(s => s.Id == ns.Id).FirstOrDefault() != null));
            Assert.AreEqual(0, _context.Routes.Count());
        }

        [Test]
        public void UpdateStation()
        {
            var newStation = new StationDTO() {Id = 1, Bikes = 12};

            var response = _controller.UpdateStation(1, newStation) as OkResult;

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(newStation.Bikes, _context.Stations.Where(s => s.Id == newStation.Id).FirstOrDefault().AvailableBikes);
            Assert.Catch<Exception404>(() =>
            {
                _controller.UpdateStation(12, new StationDTO());
            });
        }

        [Test]
        public void ReportStation()
        {
            var response = _controller.ReportStation(1) as OkResult;

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(1, _context.Stations.Where(s => s.Id == 1).FirstOrDefault().Reports);
            Assert.AreEqual(1, _context.Stations.Where(s => s.Id == 1).FirstOrDefault().State);
            Assert.Catch<Exception404>(() =>
            {
                _controller.ReportStation(12);
            });
        }

        [Test]
        public void SetStationAsBroken()
        {
            var response = _controller.SetStationAsBroken(2) as OkResult;

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(2, _context.Stations.Where(s => s.Id == 2).FirstOrDefault().State);
            Assert.AreEqual(1, _context.Stations.Where(s => s.Id == 2).FirstOrDefault().Reports);
            Assert.Catch<Exception404>(() =>
            {
                _controller.SetStationAsBroken(12);
            });
        }
        [Test]
        public void SetStationAsWorking()
        {
            var response = _controller.SetStationAsWorking(3) as OkResult;

            Assert.AreEqual(200, response.StatusCode);
            Assert.AreEqual(0, _context.Stations.Where(s => s.Id == 3).FirstOrDefault().State);
            Assert.AreEqual(0, _context.Stations.Where(s => s.Id == 3).FirstOrDefault().Reports);
            Assert.Catch<Exception404>(() =>
            {
                _controller.SetStationAsWorking(12);
            });
        }
    }
}