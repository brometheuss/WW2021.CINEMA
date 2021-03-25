using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class SeatService : ISeatService
    {
        private readonly ISeatsRepository _seatsRepository;

        public SeatService(ISeatsRepository seatsRepository)
        {
            _seatsRepository = seatsRepository;
        }

        public async Task<IEnumerable<SeatDomainModel>> GetAllAsync()
        {
            var data = await _seatsRepository.GetAll();

            List<SeatDomainModel> result = new List<SeatDomainModel>();
            SeatDomainModel model;
            foreach (var item in data)
            {
                model = new SeatDomainModel
                {
                    Id = item.Id,
                    AuditoriumId = item.AuditoriumId,
                    Number = item.Number,
                    Row = item.Row
                };
                result.Add(model);
            }

            return result;
        }

        public async Task<IEnumerable<SeatDomainModel>> GetAllSeatsByAuditoriumId(int auditoriumId)
        {
            var seatsInAuditorium = await _seatsRepository.GetSeatsByAuditoriumId(auditoriumId);

            if (seatsInAuditorium.Count() == 0)
            {
                return null;
            }

            IEnumerable<SeatDomainModel> seatDomainModels = seatsInAuditorium.Select(seat => new SeatDomainModel
            {
                Id = seat.Id,
                Row = seat.Row,
                Number = seat.Number,
                AuditoriumId = seat.AuditoriumId
            });

            return seatDomainModels;
        }

        public async Task<SeatAuditoriumDomainModel> GetAllSeatsForAuditorium(int auditoriumId)
        {
            var seatsInAuditorium = await _seatsRepository.GetSeatsByAuditoriumId(auditoriumId);

            if (seatsInAuditorium.Count() == 0)
            {
                return null;
            }

            int maxRow = seatsInAuditorium.Max(seat => seat.Row);
            int maxNumber = seatsInAuditorium.Max(seat => seat.Number);

            SeatAuditoriumDomainModel seatAuditoriumDomainModel = new SeatAuditoriumDomainModel();

            seatAuditoriumDomainModel.Seats = seatsInAuditorium.Select(seat => new SeatDomainModel
            {
                Id = seat.Id,
                Number = seat.Number,
                Row = seat.Row,
                AuditoriumId = seat.AuditoriumId
            }).ToList();

            seatAuditoriumDomainModel.MaxNumber = maxNumber;
            seatAuditoriumDomainModel.MaxRow = maxRow;

            return seatAuditoriumDomainModel;
        }
    }
}
