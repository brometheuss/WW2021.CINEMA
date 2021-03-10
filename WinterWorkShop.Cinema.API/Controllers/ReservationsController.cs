using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    }
}
