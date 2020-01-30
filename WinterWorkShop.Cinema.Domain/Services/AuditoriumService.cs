using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class AuditoriumService : IAuditoriumService
    {
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ICinemasRepository _cinemasRepository;
        private readonly ISeatsRepository _seatsRepository;

        public AuditoriumService(IAuditoriumsRepository auditoriumsRepository, ICinemasRepository cinemasRepository, ISeatsRepository seatsRepository)
        {
            _auditoriumsRepository = auditoriumsRepository;
            _cinemasRepository = cinemasRepository;
            _seatsRepository = seatsRepository;
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
                    CinemaId = item.CinemaId,
                    Name = item.AuditName
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<CreateAuditoriumResultModel> AddAuditorium(AuditoriumDomainModel domainModel,int numberOfRows, int numberOfSeats) 
        {
            var cinema = await _cinemasRepository.GetByIdAsync(domainModel.CinemaId);
            if (cinema == null)
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_UNVALID_CINEMAID
                };
            }

            var sameAuditoriumName = _auditoriumsRepository.GetByAuditName(domainModel.Name).ToList();
            if (sameAuditoriumName != null && sameAuditoriumName.Count > 0) 
            {
                return new CreateAuditoriumResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.AUDITORIUM_SAME_NAME
                };
            }

            Auditorium newAuditorium = new Auditorium
            {
                AuditName = domainModel.Name,
                CinemaId = domainModel.CinemaId,
            };

            Auditorium insertedAuditorium = _auditoriumsRepository.Insert(newAuditorium);

            if (insertedAuditorium == null)
            {
                return null;
            }

            _auditoriumsRepository.Save();

            List<SeatDomainModel> seatList = new List<SeatDomainModel>();

            for (int i = 0; i < numberOfRows; i++)
            {
                for (int j = 0; j < numberOfSeats; j++)
                {
                    Seat newSeat = new Seat()
                    {
                        AuditoriumId = insertedAuditorium.Id,
                        Row = i+1,
                        Number = j+1,
                    };

                    Seat insertedSeat = _seatsRepository.Insert(newSeat);

                    if (insertedSeat == null)
                    {
                        return null;
                    }

                    SeatDomainModel seatDomainModel = new SeatDomainModel
                    {
                        Id = insertedSeat.Id,
                        AuditoriumId = insertedSeat.AuditoriumId,
                        Row = insertedSeat.Row,
                        Number = insertedSeat.Number,
                    };

                    seatList.Add(seatDomainModel);
                }

            }

            _auditoriumsRepository.Save();

            CreateAuditoriumResultModel resultModel = new CreateAuditoriumResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Id = insertedAuditorium.Id,
                AuditoriumName = insertedAuditorium.AuditName,
                CinemaId = insertedAuditorium.CinemaId,
                CinemaName = cinema.Name,                
                SeatsList = seatList,
            };

            return resultModel;
        }
    }
}
