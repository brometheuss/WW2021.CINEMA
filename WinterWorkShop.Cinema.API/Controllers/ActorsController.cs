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
    public class ActorsController : ControllerBase
    {
        private readonly IActorService _actorService;

        public ActorsController(IActorService actorService)
        {
            _actorService = actorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ActorDomainModel>>> GetAsync()
        {
            IEnumerable<ActorDomainModel> actorDomainModels;

            actorDomainModels = await _actorService.GetAllAsync();

            if (actorDomainModels == null)
            {
                actorDomainModels = new List<ActorDomainModel>();
            }

            return Ok(actorDomainModels);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActorDomainModel>> GetById(Guid id)
        {
            ActorDomainModel actorDomainModel = await _actorService.GetByIdAsync(id);

            if (actorDomainModel == null)
            {
                return NotFound(Messages.ACTOR_NOT_FOUND);
            }

            return Ok(actorDomainModel);

        }
    }
}
