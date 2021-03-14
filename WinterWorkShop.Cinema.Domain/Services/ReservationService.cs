using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ISeatsRepository _seatRepository;
        private readonly IProjectionsRepository _projectionRepository;

        public ReservationService(IReservationsRepository reservationsRepository, ISeatsRepository seatRepository, IProjectionsRepository projectionRepository)
        {
            _reservationsRepository = reservationsRepository;
            _seatRepository = seatRepository;
            _projectionRepository = projectionRepository;
        }

        public ReservationResultModel CreateReservation(CreateReservationModel reservation)
        {
            //get all taken seats for projection
            var takenSeats = GetTakenSeats(reservation.ProjectionId);

            var seats = _seatRepository.GetAll().Result;
            var projection = _projectionRepository.GetByIdAsync(reservation.ProjectionId).Result;

            //get all seats for auditorium
            seats = seats.Where(auditorium => auditorium.AuditoriumId == projection.AuditoriumId);

            //check if the requested seats exist in the auditorium
            var commonSeats = seats.Select(x => x.Id).Intersect(reservation.SeatsRequested.Select(s => s.Id));
            if(commonSeats.Count() != reservation.SeatsRequested.Count())
            {
                return new ReservationResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.SEAT_SEATS_NOT_EXIST_FOR_AUDITORIUM
                };
            }

            //check if requested seats are already taken
            foreach(var takenSeat in takenSeats)
            {
                foreach(var requestedSeat in reservation.SeatsRequested)
                {
                    if (takenSeat.SeatDomainModel.Id == requestedSeat.Id)
                    {
                        return new ReservationResultModel
                        {
                            IsSuccessful = false,
                            ErrorMessage = Messages.SEAT_SEATS_ALREADY_TAKEN_ERROR
                        };
                    }
                }
            }

            //improvised fake payment system
            Levi9PaymentService payment = new Levi9PaymentService();
            var paymentResult = payment.MakePayment().Result;

            if(paymentResult.IsSuccess == false)
            {
                return new ReservationResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PAYMENT_CREATION_ERROR
                };
            }
            //end of payment system


            //create reservation for inserting
            Reservation reservationToAdd = new Reservation
            {
                Id = Guid.NewGuid(),
                ProjectionId = reservation.ProjectionId,
                UserId = reservation.UserId
            };

            var insertedReservation = _reservationsRepository.Insert(reservationToAdd);

            foreach(var rs in reservation.SeatsRequested)
            {
                reservationToAdd.ReservationSeats.Add(new ReservationSeat
                {
                    SeatId = rs.Id,
                    ReservationId = insertedReservation.Id
                });
            }

            _reservationsRepository.Save();

            return new ReservationResultModel
            {
                IsSuccessful = true,
                Reservation = new ReservationDomainModel
                {
                    Id = insertedReservation.Id,
                    ProjectionId = insertedReservation.ProjectionId,
                    UserId = insertedReservation.UserId
                }
            };
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

        public IEnumerable<SeatResultModel> GetTakenSeats(Guid projectionId)
        {
            var reservations = _reservationsRepository.GetReservationByProjectionId(projectionId);

            if(reservations == null)
            {
                return null;
            }

            List<SeatResultModel> seatList = new List<SeatResultModel>();

            foreach(var reservation in reservations)
            {
                var seats = reservation.ReservationSeats.Select(s => new SeatResultModel
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
                foreach(var seat in seats)
                {
                    seatList.Add(seat);
                }
            }

            return seatList;
        }
    }
}
