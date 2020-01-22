using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
        }

        public async Task<IEnumerable<AuditoriumDomainModel>> GetAllAsync()
        {
            var data = await _auditoriumsRepository.GetAll();

            if (data == null)
            {
                return null;                
            }

            List<AuditoriumDomainModel> result = new List<AuditoriumDomainModel>();
            AuditoriumDomainModel model;
            foreach (var item in data)
            {
                model = new AuditoriumDomainModel
                {
                    Id = item.Id,
                    CinemaId = item.CinemaId
                };
                result.Add(model);
            }

            return result;
        }
    }
}
