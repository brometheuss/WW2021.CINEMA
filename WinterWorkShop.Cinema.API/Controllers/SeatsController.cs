using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class SeatsController : ControllerBase
    {
        private readonly ISeatService _seatService;
        private readonly IReservationService _reservationService;

        public SeatsController(ISeatService seatService, IReservationService reservationService)
        {
            _seatService = seatService;
            _reservationService = reservationService;
        }

        /// <summary>
        /// Gets all seats
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetAsync()
        {
            IEnumerable<SeatDomainModel> seatDomainModels;
            
            seatDomainModels = await _seatService.GetAllAsync();

            if (seatDomainModels == null)
            {
                seatDomainModels = new List<SeatDomainModel>();
            }

            return Ok(seatDomainModels);
        }

        [HttpGet]
        [Route("{projectionId:guid}")]
        public ActionResult<IEnumerable<SeatDomainModel>> GetTakenSeats(Guid projectionId)
        {
            var takenSeats = _reservationService.GetTakenSeats(projectionId);

            List<SeatDomainModel> seatDomainList = new List<SeatDomainModel>();

            foreach(var seat in takenSeats)
            {
                SeatDomainModel s = new SeatDomainModel
                {
                    AuditoriumId = seat.SeatDomainModel.AuditoriumId,
                    Id = seat.SeatDomainModel.Id,
                    Number = seat.SeatDomainModel.Number,
                    Row = seat.SeatDomainModel.Row
                };
                seatDomainList.Add(s);
            }

            return Ok(seatDomainList);
        }
    }
}
