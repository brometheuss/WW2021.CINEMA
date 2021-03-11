using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class CityService : ICityService
    {
        private readonly ICitiesRepository _citiesRepository;

        public CityService(ICitiesRepository citiesRepository)
        {
            _citiesRepository = citiesRepository;
        }

        public async Task<IEnumerable<CityDomainModel>> GetAllAsync()
        {
            var cities = await _citiesRepository.GetAll();

            if (cities == null)
            {
                return null;
            }

            List<CityDomainModel> cityList = new List<CityDomainModel>();

            foreach(var city in cities)
            {
                CityDomainModel cityModel = new CityDomainModel
                {
                    Id = city.Id,
                    Name = city.Name,
                    CinemasList = new List<CinemaDomainModel>()
                };

                foreach(var cinema in city.Cinemas)
                {
                    CinemaDomainModel cinemaModel = new CinemaDomainModel
                    {
                        Id = cinema.Id,
                        CityId = city.Id,
                        Name = cinema.Name
                    };

                    cityModel.CinemasList.Add(cinemaModel);
                }

                cityList.Add(cityModel);
            }

            return cityList;
        }

        public async Task<CityDomainModel> GetByIdAsync(int id)
        {
            var city = await _citiesRepository.GetByIdAsync(id);

            if(city == null)
            {
                return null;
            }

            CityDomainModel cityModel = new CityDomainModel
            {
                Id = city.Id,
                Name = city.Name,
                CinemasList = new List<CinemaDomainModel>()
            };
            
            foreach(var cinema in city.Cinemas)
            {
                CinemaDomainModel cinemaModel = new CinemaDomainModel
                {
                    Id = cinema.Id,
                    CityId = city.Id,
                    Name = cinema.Name
                };

                cityModel.CinemasList.Add(cinemaModel);
            }




            

            return cityModel;
        }
    }
}
