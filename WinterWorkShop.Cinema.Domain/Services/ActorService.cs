using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class ActorService : IActorService
    {
        private readonly IActorsRepository _actorsRepository;

        public ActorService(IActorsRepository citiesRepository)
        {
            _actorsRepository = citiesRepository;
        }

        public async Task<IEnumerable<ActorDomainModel>> GetAllAsync()
        {
            var actors = await _actorsRepository.GetAll();

            if (actors == null)
            {
                return null;
            }

            List<ActorDomainModel> actorList = new List<ActorDomainModel>();

            foreach (var actor in actors)
            {
                ActorDomainModel actorModel = new ActorDomainModel
                {
                    Id = actor.Id,
                    FirstName = actor.FirstName,
                    LastName = actor.LastName
                };

                actorList.Add(actorModel);
            }

            return actorList;
        }

        public async Task<ActorDomainModel> GetByIdAsync(Guid id)
        {
            var actor = await _actorsRepository.GetByIdAsync(id);

            if (actor == null)
            {
                return null;
            }

            ActorDomainModel actorModel = new ActorDomainModel
            {
                Id = actor.Id,
                FirstName = actor.FirstName,
                LastName = actor.LastName
            };

            return actorModel;
        }
    }
}
