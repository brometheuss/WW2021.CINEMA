using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationsRepository _reservationsRepository;

        public ReservationService(IReservationsRepository reservationsRepository)
        {
            _reservationsRepository = reservationsRepository;
        }

        public async Task<IEnumerable<ReservationDomainModel>> GetAllAsync()
        {
            var reservations = await _reservationsRepository.GetAll();

            if (reservations == null)
            {
                return null;
            }

            List<ReservationDomainModel> reservationList = new List<ReservationDomainModel>();

            foreach (var reservation in reservations)
            {
                ReservationDomainModel reservationModel = new ReservationDomainModel
                {
                    Id = reservation.Id,
                    ProjectionId = reservation.ProjectionId,
                    UserId = reservation.UserId
                };


                reservationList.Add(reservationModel);
            }

            return reservationList;
        }

        public async Task<ReservationDomainModel> GetByIdAsync(Guid id)
        {
            var reservation = await _reservationsRepository.GetByIdAsync(id);

            if (reservation == null)
            {
                return null;
            }

            ReservationDomainModel reservationModel = new ReservationDomainModel
            {
                Id = reservation.Id,
                ProjectionId = reservation.ProjectionId,
                UserId = reservation.UserId
            };

            return reservationModel;
        }

        public IEnumerable<SeatResultModel> GetTakenSeats(int projectionId)
        {
            var reservation = _reservationsRepository.GetReservationByProjectionId(projectionId);

            if(reservation == null)
            {
                return null;
            }

            return reservation.ReservationSeats.Select(s => new SeatResultModel
            {
                IsSuccessful = true,
                SeatDomainModel = new SeatDomainModel
                {
                    Id = s.SeatId,
                    AuditoriumId = s.Seat.AuditoriumId,
                    Number = s.Seat.Number,
                    Row = s.Seat.Row
                }
            });
        }
    }
}
