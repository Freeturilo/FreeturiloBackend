using Moq;
using NUnit.Framework;
using NextBikeApiService;
using NextBikeApiService.Helpers;
using Microsoft.Extensions.Logging;
using System.Threading;
using System;
using NextBikeDataParser;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using FreeturiloWebApi.DTO;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using FreeturiloWebApi;
using FreeturiloWebApi.HttpMethods;

namespace NextBikeApiService.Test
{
    public class Tests
    {
        class FakeNbHandler : INextBikeApiHandler
        {
            public markers GetNextBikeData(ILogger logger, bool readFromDump, string xsdPath = null, string dumpPath = null, string url = null, string parameters = null)
            {
                return new markers
                {
                    country = new markersCountry
                    {
                        city = new markersCountryCity
                        {
                            place = new markersCountryCityPlace[]
                            {
                                new markersCountryCityPlace
                                {
                                    uid = 1,
                                    lat = 1,
                                    lng = 1,
                                    bikes_available_to_rent = 13,
                                    free_racks = 23,
                                }
                            }
                        }
                    }
                };
            }
        }
        class FakeAppMethods : IAppMethods
        {
            int state = 0;
            public void Demo(string serverPath, string token)
            {
                state = 1;
            }

            public void GetReportTrashold(string serverPath, string token)
            {
                throw new NotImplementedException();
            }

            public void SetReportTrashold(string serverPath, string token, int number)
            {
                throw new NotImplementedException();
            }

            public void Start(string serverPath, string token)
            {
                state = 0;
            }

            public int Status(string serverPath, string token)
            {
                return state;
            }

            public void Stop(string serverPath, string token)
            {
                state = 2;
            }

            int IAppMethods.GetReportTrashold(string serverPath, string token)
            {
                throw new NotImplementedException();
            }
        }
        class FakeUserMethods : IUserMethods
        {
            readonly bool exception = false;
            public FakeUserMethods(bool exception = false)
            {
                this.exception = exception;
            }
            public string Authenticate(string serverPath, AuthDTO auth)
            {
                if (exception) throw new Exception();
                return "token";
            }
        }
        class FakeStationMethods : IStationMethods
        {
            public static int counter = 0;
            public StationDTO[] GetAllStations(string serverPath)
            {
                return new StationDTO[]
                {
                    new StationDTO
                    {
                        Id = 1,
                        Latitude = 1,
                        Longitude = 1,
                        Bikes = 13,
                        BikeRacks= 24
                    }
                };
            }

            public StationDTO GetStation(string serverPath, int id)
            {
                throw new NotImplementedException();
            }

            public void ReportStation(string serverPath, int id)
            {
                throw new NotImplementedException();
            }

            public void SetStationAsBroken(string serverPath, string token, int id)
            {
                throw new NotImplementedException();
            }

            public void SetStationAsWorking(string serverPath, string token, int id)
            {
                throw new NotImplementedException();
            }

            public void UpdateAllStations(string serverPath, string token, StationDTO[] newStations)
            {
                counter++;
            }
        }

        /// <summary>
        /// Test checks if BikeDataComparer works properly
        /// </summary>
        [Test]
        public void BikeDataComparerTest()
        {
            markers markers = new markers
            {
                country = new markersCountry
                {
                    city = new markersCountryCity
                    {
                        place = new markersCountryCityPlace[]
                        {
                            new markersCountryCityPlace
                            {
                                uid = 1,
                                lat = 10,
                                lng = 10,
                                bikes_available_to_rent = 11,
                                free_racks = 15,
                            },
                            new markersCountryCityPlace
                            {
                                uid = 2,
                                lat = 10,
                                lng = 10,
                                bikes_available_to_rent = 11,
                                free_racks = 15,
                            },
                            new markersCountryCityPlace
                            {
                                uid = 3,
                                lat = 10,
                                lng = 10,
                                bikes_available_to_rent = 11,
                                free_racks = 15,
                            }
                        }
                    }
                }
            };

            var stations = new StationDTO[]
            {
                new StationDTO
                {
                    Id = 1,
                    Latitude = 10,
                    Longitude = 10,
                    Bikes = 11,
                    BikeRacks = 15,
                },
                new StationDTO
                {
                    Id = 2,
                    Latitude = 10,
                    Longitude = 10,
                    Bikes = 14,
                    BikeRacks = 15,
                },
                new StationDTO
                {
                    Id = 3,
                    Latitude = 10,
                    Longitude = 10,
                    Bikes = 11,
                    BikeRacks = 15,
                },
                new StationDTO
                {
                    Id = 4,
                    Latitude = 10,
                    Longitude = 10,
                    Bikes = 11,
                    BikeRacks = 15,
                },
            };
            var res = BikeDataComparer.Compare(markers, stations);
            Assert.AreEqual(res.Count(), 1);
        }

        /// <summary>
        /// Test checks if NextBikeApiHandler builds and executes properly
        /// </summary>
        [Test]
        public void NextBikeApiHandlerTest()
        {
            var nbHandler = new NextBikeApiHandler();

            var loggerFactory = LoggerFactory.Create(builder =>
            {
                builder
                    .AddConsole()
                    .AddEventLog();
            });
            ILogger logger = loggerFactory.CreateLogger<IWorkerHandler>();

            nbHandler.GetNextBikeData(logger, true, "../../../../NextBikeDataParser/markers.xsd", @"..\..\..\..\NextBikeApiService\xml-dumps");
            nbHandler.GetNextBikeData(logger, false, "../../../../NextBikeDataParser/markers.xsd");
            nbHandler.GetNextBikeData(logger, false, "../../../../NextBikeDataParser/markers.xsd", null, "www.fake.pl", "?city=12");

            Assert.Catch<System.IO.IOException>(() =>
            {
                nbHandler.GetNextBikeData(logger, true, null);
            });
        }

        /// <summary>
        /// Test checks if worker calls proper methods
        /// </summary>
        [Test]
        public void WorkerHandlerTest()
        {

            var nbHandler = new FakeNbHandler();

            var loggerFactory = LoggerFactory.Create(builder =>
                {
                    builder
                        .AddConsole()
                        .AddEventLog();
                });
            var logger = loggerFactory.CreateLogger<WorkerHandler>();

            var appMths = new FakeAppMethods();
            var userMths = new FakeUserMethods();
            var stationMths = new FakeStationMethods();
            var worker = new WorkerHandler(logger, nbHandler, userMths, appMths, stationMths);
            worker.Work();
            Assert.AreEqual(FakeStationMethods.counter, 1);

            appMths.Demo("path", "token");
            worker.Work();
            Assert.AreEqual(FakeStationMethods.counter, 2);

            appMths.Stop("path", "token");
            worker.Work();
            Assert.AreEqual(FakeStationMethods.counter, 2);


            var userMeths2 = new FakeUserMethods(true);
            var worker2 = new WorkerHandler(logger, nbHandler, userMeths2, appMths, stationMths);
            appMths.Start("path", "token");
            worker2.Work();
            Assert.AreEqual(FakeStationMethods.counter, 3);

            var stationMths2 = new StationMethods();
            var worker3 = new WorkerHandler(logger, nbHandler, userMths, appMths, stationMths2);
            worker3.Work();
            Assert.AreEqual(FakeStationMethods.counter, 3);
        }
    }
}