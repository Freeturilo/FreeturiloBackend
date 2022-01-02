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
using System.IO;
using System.Text.Json;
using FreeturiloWebApi.DTO.GoogleDTO;
using System.Text;
using System.Collections.Generic;

namespace FreeturiloWebApi.Test
{
    public class GoogleMapsApiTests
    {
        [Test]
        public void Desarialize()
        {
            var json = File.ReadAllText(@"..\..\..\RouteDTOSerialized.txt", Encoding.UTF8).Replace("\r\n", "").Replace(" ", "");
            var obj = JsonSerializer.Deserialize<FragmentRouteDTO>(json);
            var bytes = JsonSerializer.SerializeToUtf8Bytes(obj, typeof(FragmentRouteDTO));
            var serialized = Encoding.UTF8.GetString(bytes).Replace("\r\n", "").Replace(" ", "");
            Assert.AreEqual(json, serialized);

            json = File.ReadAllText(@"..\..\..\DirectionDTOSerialized.txt", Encoding.UTF8);
            var obj2 = JsonSerializer.Deserialize<DirectionDTO>(json);
            bytes = JsonSerializer.SerializeToUtf8Bytes(obj2, typeof(DirectionDTO));
            serialized = Encoding.UTF8.GetString(bytes).Replace("\r\n", "").Replace(" ", "");
            Assert.AreEqual(json, serialized);
        }

        [Test]
        public void GetRoute()
        {
            LocationDTO loc1 = new LocationDTO()
            {
                Longitude = 20.97481,
                Latitude = 52.220877,
                Name = "DS Akademik",
                Type = "Location",
            };
            LocationDTO loc2 = new LocationDTO()
            {
                Longitude = 21.043405,
                Latitude = 52.249512,
                Name = "DS Riviera",
                Type = "Location",
            };
            StationDTO st1 = new StationDTO()
            {
                Id = 11,
                Longitude = 21.0000,
                Latitude = 52.23,
                Name = "Stacja",
                Type = "Station",
                Bikes = 10,
                BikeRacks = 10,
            };
            StationDTO st2 = new StationDTO()
            {
                Id = 12,
                Longitude = 21.0000,
                Latitude = 52.23,
                Name = "Stacja",
                Type = "Station",
                Bikes = 10,
                BikeRacks = 10,
            };

            var r1 = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { loc1, st1, loc2 }, "bicycling");
            var r2 = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { loc1, loc2 }, "bicycling");
            var r3 = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { loc1, st1, loc2, st1, loc1, st2, loc2 }, "bicycling");
            var r4 = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { loc1, st1, loc2, loc1, st2, loc2 }, "bicycling");
            var r5 = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { loc1, loc2 }, "walking");
            var r6 = GoogleMapsAPIHandler.GetRoute(new List<LocationDTO>() { loc1, loc2, loc1, loc2 }, "walking");

            Assert.AreEqual(r1.Waypoints.Length, 3);
            Assert.AreEqual(r2.Waypoints.Length, 2);
            Assert.AreEqual(r3.Waypoints.Length, 7);
            Assert.AreEqual(r4.Waypoints.Length, 6);
            Assert.IsTrue(r4.Cost > 0);
            Assert.IsTrue(r4.Time > 20 * 60);
            Assert.AreEqual(r5.Waypoints.Length, 2);
            Assert.AreEqual(r6.Waypoints.Length, 4);
            Assert.AreEqual(r5.Cost, 0);
            Assert.AreEqual(r6.Cost, 0);
        }
    }
}