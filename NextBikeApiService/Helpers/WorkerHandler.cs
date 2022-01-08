using FreeturiloWebApi.HttpMethods;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBikeApiService.Helpers
{
    public class WorkerHandler : IWorkerHandler
    {

        private static readonly string serverPath = @"https://freeturilowebapi.azurewebsites.net/";
        private readonly ILogger<WorkerHandler> _logger;
        private readonly INextBikeApiHandler _nbHandler;
        private readonly IUserMethods _userMethods;
        private readonly IAppMethods _appMethods;
        private readonly IStationMethods _stationMethods;

        /// <summary>
        /// Email and password for admin to authenticate to update Freeturilo database
        /// </summary>
        private const string email = "freeturilo@gmail.com";
        private string password = File.ReadAllText(".admin-passwd");

        public WorkerHandler(ILogger<WorkerHandler> logger, INextBikeApiHandler nbHandler, IUserMethods userMethods, IAppMethods appMethods, IStationMethods stationMethods)
        {
            _logger = logger;
            _nbHandler = nbHandler;
            _userMethods = userMethods;
            _appMethods = appMethods;
            _stationMethods = stationMethods;
        }
        /// <summary>
        /// Method to launch worker
        /// </summary>
        public void Work()
        {
            int appState = 1;
            string token = null;

            try
            {
                token = _userMethods.Authenticate(serverPath, new() { Email = email, Password = password });
                appState = _appMethods.Status(serverPath, token);
            }
            catch
            {
                _logger.LogError("Could not get token or server state");
            }

            if (appState == 2)
            {
                _logger.LogError("Server is stopped");
            }
            else
            {
                try
                {
                    bool readFromDump = appState == 1;
                    var markers = _nbHandler.GetNextBikeData(_logger, readFromDump, null, null);
                    var freeturiloStations = _stationMethods.GetAllStations(serverPath);
                    var toBeUpdated = BikeDataComparer.Compare(markers, freeturiloStations);
                    _stationMethods.UpdateAllStations(serverPath, token, toBeUpdated.ToArray());
                    _logger.LogInformation("Stations updated");
                }
                catch
                {
                    _logger.LogInformation("Could not update stations");
                }
            }
        }
    }
}
