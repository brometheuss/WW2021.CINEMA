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
    public class CitiesController : ControllerBase
    {
        private readonly ICityService _cityService;

        public CitiesController(ICityService cityService)
        {
            _cityService = cityService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDomainModel>>> GetAsync()
        {
            IEnumerable<CityDomainModel> cityDomainModels;

            cityDomainModels = await _cityService.GetAllAsync();

            if (cityDomainModels == null)
            {
                cityDomainModels = new List<CityDomainModel>();
            }

            return Ok(cityDomainModels);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<CityDomainModel>> GetById(int id)
        {
            CityDomainModel cityDomainModel = await _cityService.GetByIdAsync(id);

            if(cityDomainModel == null)
            {
                return NotFound(Messages.CITY_NOT_FOUND);
            }

            return Ok(cityDomainModel);

        }



    }
}
