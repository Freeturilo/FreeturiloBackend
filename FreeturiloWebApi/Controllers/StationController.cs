using FreeturiloWebApi.Attributes;
using FreeturiloWebApi.DTO;
using FreeturiloWebApi.Models;
using FreeturiloWebApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FreeturiloWebApi.Controllers
{
    [Controller]
    [Route("/station")]
    public class StationController : AuthController
    {
        private readonly IStationService _service;
        public StationController(IStationService service)
        {
            _service = service;
        }
        /// <summary>
        /// Returns all stations
        /// </summary>
        /// <returns>Returns all stations</returns>
        /// <response code="200">Success</response>
        /// <response code="503">Service unavailable</response>
        [HttpGet]
        [AppState]
        public ActionResult<StationDTO[]> GetAllStations()
        {
            var stations = _service.GetAllStations();
            return Ok(stations);
        }
        /// <summary>
        /// Updates all stations
        /// </summary>
        /// <param name="newStations">Collection of stations to be updated</param>
        /// <returns>Returns information if stations has been updated properly</returns>
        /// <response code="200">Success</response>
        /// <response code="400">Bad Request</response>
        /// <response code="401">Unauthorized</response>
        [HttpPut]
        [Auth]
        public ActionResult UpdateAllStations([FromBody] StationDTO[] newStations)
        {
            _service.UpdateAllStations(newStations);
            return Ok();
        }
        /// <summary>
        /// Returns station of given id
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns>Returns details of the station</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        /// <response code="503">Service unavailable</response>
        [HttpGet("{id}")]
        [AppState]
        public ActionResult<StationDTO> GetStation([FromRoute] int id)
        {
            var station = _service.GetStation(id);
            return Ok(station);
        }
        /// <summary>
        /// Reports station of given id as possibly broken
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns>Reports station</returns>
        /// <response code="200">Success</response>
        /// <response code="404">Not found</response>
        /// <response code="503">Service unavailable</response>
        [HttpPost("{id}/report")]
        [AppState]
        public ActionResult ReportStation([FromRoute] int id)
        {
            _service.ReportStation(id);
            return Ok();
        }
        /// <summary>
        /// Sets station of given id as broken
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns>Sets station as broken</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpPost("{id}/broken")]
        [Auth]
        public ActionResult SetStationAsBroken([FromRoute] int id)
        {
            _service.SetStationAsBroken(id);
            return Ok();
        }
        /// <summary>
        /// Sets station of given id as working
        /// </summary>
        /// <param name="id">Station id</param>
        /// <returns>Sets station as working</returns>
        /// <response code="200">Success</response>
        /// <response code="401">Unauthorized</response>
        /// <response code="404">Not found</response>
        [HttpPost("{id}/working")]
        [Auth]
        public ActionResult SetStationAsWorking([FromRoute] int id)
        {
            _service.SetStationAsWorking(id);
            return Ok();
        }
    }
}
