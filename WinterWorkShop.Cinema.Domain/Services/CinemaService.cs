using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class CinemaService : ICinemaService
    {
        private readonly ICinemasRepository _cinemasRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ISeatsRepository _seatsRepository;
        private readonly ICitiesRepository _citiesRepository;


        public CinemaService(
            ICinemasRepository cinemasRepository, 
            IAuditoriumsRepository auditoriumsRepository,
            ISeatsRepository seatsRepository,
            ICitiesRepository citiesRepository
            )
        {
            _cinemasRepository = cinemasRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _seatsRepository = seatsRepository;
            _citiesRepository = citiesRepository;
        }

        public async Task<CinemaDomainModel> CreateCinemaAsync(CinemaDomainModel cinemaDomainModel, int numOfSeats, int numOfRows)
        {
            var city = await _citiesRepository.GetByIdAsync(cinemaDomainModel.CityId);

            if(city == null)
            {
                return null;
            }

            var cinemas = await _cinemasRepository.GetAll();
            var cinemasInSameCity = cinemas.Where(c => c.CityId == cinemaDomainModel.CityId);

            foreach(var cin in cinemasInSameCity)
            {
               if(cin.Name == cinemaDomainModel.Name)
                {
                    return null;
                }
            }

            Data.Cinema newCinema = new Data.Cinema
            {
                Name = cinemaDomainModel.Name,
                CityId = cinemaDomainModel.CityId
            };

            newCinema.Auditoriums = new List<Auditorium>();

                Auditorium auditorium = new Auditorium
                {
                    AuditName = nameof(Auditorium) + " Default" 
                };

                auditorium.Seats = new List<Seat>();

                for(int j = 1; j <= numOfRows; j++)
                {
                    for(int k = 1; k <= numOfSeats; k++)
                    {
                        Seat seat = new Seat
                        {
                            Row = j,
                            Number = k
                        };

                        auditorium.Seats.Add(seat);
                    }
                }

                newCinema.Auditoriums.Add(auditorium); 
            

            Data.Cinema insertedCinema = _cinemasRepository.Insert(newCinema);

            if(insertedCinema == null)
            {
                return null;
            }

            _cinemasRepository.Save();

            if(insertedCinema == null)
            {
                return null;
            }

            CinemaDomainModel cinemaModel = new CinemaDomainModel
            {
                Id = insertedCinema.Id,
                Name = insertedCinema.Name,
                CityId = insertedCinema.CityId,
                AuditoriumsList = new List<AuditoriumDomainModel>()
            };

            foreach(var auditoriumInserted in insertedCinema.Auditoriums)
            {
                AuditoriumDomainModel modelAuditorium = new AuditoriumDomainModel
                {
                    Id = auditoriumInserted.Id,
                    CinemaId = insertedCinema.Id,
                    Name = auditoriumInserted.AuditName,
                    SeatsList = new List<SeatDomainModel>()
                };

                foreach(var seat in auditoriumInserted.Seats)
                {
                    modelAuditorium.SeatsList.Add(new SeatDomainModel 
                    { 
                        Id = seat.Id,
                        AuditoriumId = auditoriumInserted.Id,
                        Number = seat.Number,
                        Row = seat.Row
                    });
                }

                cinemaModel.AuditoriumsList.Add(modelAuditorium);

            }

            return cinemaModel;
        }

        public async Task<CinemaDomainModel> DeleteCinemaAsync(int id)
        {
            var cinema =  await _cinemasRepository.GetByIdAsync(id);

            if(cinema == null)
            {
                return null;
            }

            var allAuditoriums = await _auditoriumsRepository.GetAll();

            if(allAuditoriums == null)
            {
                return null;
            }

            var auditoriumsInCinema = allAuditoriums.Where(a => a.CinemaId == id);

            var seats = await _seatsRepository.GetAll();

            foreach (var auditorium in auditoriumsInCinema)
            {
                seats = seats.Where(s => s.AuditoriumId == auditorium.Id);

                foreach(var seat in seats)
                {
                    _seatsRepository.Delete(seat.Id);
                }

                _auditoriumsRepository.Delete(auditorium.Id);
            }

            CinemaDomainModel cinemaModel = new CinemaDomainModel
            {
                Id = cinema.Id,
                Name = cinema.Name,
                CityId = cinema.CityId,
                AuditoriumsList = cinema.Auditoriums.Select(a => new AuditoriumDomainModel
                {
                    Id = a.Id,
                    CinemaId = a.CinemaId,
                    Name = a.AuditName,
                    SeatsList = a.Seats.Select(s => new SeatDomainModel
                    {
                        Id = s.Id,
                        AuditoriumId = s.AuditoriumId,
                        Number = s.Number,
                        Row = s.Row
                    })
                    .ToList()
                })
                .ToList()
            };

            _cinemasRepository.Delete(cinema.Id);

            _cinemasRepository.Save();

            return cinemaModel;

        }

        public async Task<IEnumerable<CinemaDomainModel>> GetAllAsync()
        {
            var data = await _cinemasRepository.GetAll();
            
            if (data == null)
            {
                return null;
            }

            List<CinemaDomainModel> result = new List<CinemaDomainModel>();
            CinemaDomainModel model;
            foreach (var cinema in data)
            {
                model = new CinemaDomainModel
                {
                    Id = cinema.Id,
                    Name = cinema.Name,
                    CityId = cinema.CityId,
                    AuditoriumsList = new List<AuditoriumDomainModel>()
                };

                foreach(var auditorium in cinema.Auditoriums)
                {
                    AuditoriumDomainModel auditoriumModel = new AuditoriumDomainModel
                    {
                        Id = auditorium.Id,
                        CinemaId = cinema.Id,
                        Name = auditorium.AuditName
                    };

                    model.AuditoriumsList.Add(auditoriumModel);
                }

                result.Add(model);
            }

            return result;
        }
    }

}
