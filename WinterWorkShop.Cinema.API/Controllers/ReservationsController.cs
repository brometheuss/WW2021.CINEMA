using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReservationDomainModel>>> GetAsync()
        {
            IEnumerable<ReservationDomainModel> reservationDomainModels;

            reservationDomainModels = await _reservationService.GetAllAsync();

            if (reservationDomainModels == null)
            {
                reservationDomainModels = new List<ReservationDomainModel>();
            }

            return Ok(reservationDomainModels);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ReservationDomainModel>> GetById(Guid id)
        {
            ReservationDomainModel reservationDomainModel = await _reservationService.GetByIdAsync(id);

            if (reservationDomainModel == null)
            {
                return NotFound(Messages.RESERVATION_NOT_FOUND);
            }

            return Ok(reservationDomainModel);

        }

        [HttpPost]
        [Route("{action}")]
        public ActionResult<ReservationResultModel> MakeReservation([FromBody] CreateReservationModel model)
        {
            ReservationResultModel res;

            try
            {
                res = _reservationService.CreateReservation(model);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!res.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = res.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Created("reservations//" + res.Reservation.Id, res);
        }

        [HttpGet("getbyprojectionid/{id}")]
        public async Task<ActionResult<IEnumerable<SeatDomainModel>>> GetTakenSeats(Guid id)
        {
            var takenSeats = _reservationService.GetTakenSeats(id);

            if (takenSeats == null)
            {
                List<SeatDomainModel> list = new List<SeatDomainModel>();
                return Ok(list);
            }

            IEnumerable<SeatDomainModel> seats = takenSeats.Select(seatResult => new SeatDomainModel
            {
                Id = seatResult.SeatDomainModel.Id,
                Row = seatResult.SeatDomainModel.Row,
                Number = seatResult.SeatDomainModel.Number,
                AuditoriumId = seatResult.SeatDomainModel.AuditoriumId
            });

            return Ok(seats);
        }

        [HttpGet("byuserid/{id}")]
        public async Task<ActionResult<IEnumerable<UserReservationDomainModel>>> GetUserReservationsByUserId(Guid id)
        {
            var reservations = await _reservationService.GetReservationsByUserId(id);

            if (reservations == null)
            {
                reservations = new List<UserReservationDomainModel>();
            }

            return Ok(reservations);
        }

    }
}
