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

        [HttpGet("numberofseats/{id}")]
        public async Task<ActionResult<SeatAuditoriumDomainModel>> GetNumberOfSeats(int id)
        {
            var seats = await _seatService.GetAllSeatsForAuditorium(id);

            if (seats == null)
            {
                return NotFound(Messages.SEAT_AUDITORIUM_NOT_FOUND);
            }

            return Ok(seats);
        }

        [HttpGet("byauditoriumid/{id}")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetAllSeatsByAuditoriumId(int id)
        {
            var seats = await _seatService.GetAllSeatsByAuditoriumId(id);

            if (seats == null)
            {
                return NotFound(Messages.SEAT_AUDITORIUM_NOT_FOUND);
            }

            return Ok(seats);
        }
    }
}
