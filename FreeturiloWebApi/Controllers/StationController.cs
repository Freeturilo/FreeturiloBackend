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

        [HttpGet]
        [AppState]
        public ActionResult<StationDTO[]> GetAllStations()
        {
            var stations = _service.GetAllStations();
            return Ok(stations);
        }
        [HttpPost]
        [Auth]
        public ActionResult<StationDTO> AddNewStation([FromBody] StationDTO newStation)
        {
            var station = _service.AddNewStation(newStation);
            return Ok(station);
        }
        [HttpPut]
        [Auth]
        public ActionResult UpdateAllStations([FromBody] StationDTO[] newStations)
        {
            _service.UpdateAllStations(newStations);
            return Ok();
        }
        [HttpGet("{id}")]
        [AppState]
        public ActionResult<StationDTO> GetStation([FromRoute] int id)
        {
            var station = _service.GetStation(id);
            return Ok(station);
        }
        [HttpPut("{id}")]
        [Auth]
        public ActionResult UpdateStation([FromRoute] int id, [FromBody] StationDTO newStation)
        {
            _service.UpdateStation(id, newStation);
            return Ok();
        }
        [HttpPost("{id}/report")]
        [AppState]
        public ActionResult ReportStation([FromRoute] int id)
        {
            _service.ReportStation(id);
            return Ok();
        }
        [HttpPost("{id}/broken")]
        [Auth]
        public ActionResult SetStationAsBroken([FromRoute] int id)
        {
            _service.SetStationAsBroken(id);
            return Ok();
        }
        [HttpPost("{id}/working")]
        [Auth]
        public ActionResult SetStationAsWorking([FromRoute] int id)
        {
            _service.SetStationAsWorking(id);
            return Ok();
        }
    }
}
