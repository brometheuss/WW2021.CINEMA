using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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
    }
}
