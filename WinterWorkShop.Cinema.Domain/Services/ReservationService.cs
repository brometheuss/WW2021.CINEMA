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
        private readonly IAuditoriumsRepository _auditoriumRepository;



        public ReservationService(IReservationsRepository reservationsRepository, ISeatsRepository seatRepository, IProjectionsRepository projectionRepository, IAuditoriumsRepository auditoriumRepository)
        {
            _reservationsRepository = reservationsRepository;
            _seatRepository = seatRepository;
            _projectionRepository = projectionRepository;
            _auditoriumRepository = auditoriumRepository;
        }

        public ReservationResultModel CreateReservation(CreateReservationModel reservation)
        {
            //get all taken seats for projection
            var takenSeats = GetTakenSeats(reservation.ProjectionId);

            var seats = _seatRepository.GetAll().Result;
            var projection = _projectionRepository.GetByIdAsync(reservation.ProjectionId).Result;

            if(projection == null)
            {
                return new ReservationResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PROJECTION_DOES_NOT_EXIST
                };
            }

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


            //check if requested seats are more than 1 and in the same row
            List<SeatDomainModel> seatModels = new List<SeatDomainModel>();

            foreach (var seat in reservation.SeatsRequested)
            {
                var reqSeat = _seatRepository.GetByIdAsync(seat.Id).Result;
                SeatDomainModel seatDomain = new SeatDomainModel()
                {
                    Id = reqSeat.Id,
                    Number = reqSeat.Number,
                    Row = reqSeat.Row,
                    AuditoriumId = reqSeat.AuditoriumId
                };
                seatModels.Add(seatDomain);
            }

            if (reservation.SeatsRequested.ToList().Count() > 1)
            {
                var singleSeat = seatModels[0];
        
                foreach (var x in seatModels)
                {
                    if(singleSeat.Row != x.Row)
                    {
                        return new ReservationResultModel
                        {
                            IsSuccessful = false,
                            ErrorMessage = Messages.SEAT_SEATS_NOT_IN_SAME_ROW
                        };
                    }
                }
            }

            //check if seats are not next to each other
            seatModels = seatModels.OrderByDescending(x => x.Number).ToList();

            var singleSeat2 = seatModels[0];

            var counter = 1;
            foreach(var y in seatModels.Skip(1))
            {
                if(y.Number + counter != singleSeat2.Number)
                {
                    return new ReservationResultModel
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.SEAT_SEATS_MUST_BE_NEXT_TO_EACH_OTHER
                    };
                }
                else
                {
                    counter++;
                }
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

            #region Komentar
            /*  var auditorium = _auditoriumRepository.GetByIdAsync(projection.AuditoriumId).Result;


              List<(int, int)> requestedSeatsRowsAndNumbers =
                  reservation.SeatsRequested
                  .Select(s =>
                  {
                  var seat = _seatRepository.GetByIdAsync(s.Id).Result;

                  return (seat.Row, seat.Number);
                   }).ToList();


              int rowMax = auditorium.Seats.Max(s => s.Row);
              int numberMax = auditorium.Seats.Max(s => s.Number);


              List<(int row, int number)> listOfAllSeats = new List<(int, int)>();
              for (int i = 1; i <= rowMax; i++)
              {
                  for (int j = 1; j <= numberMax; j++)
                  {
                      listOfAllSeats.Add((i, j));
                  }
              }


              List<(int row, int number)> listTakenSeats = takenSeats
                  .Select(s => (s.SeatDomainModel.Row, s.SeatDomainModel.Number))
                  .ToList();


              List<(int row, int number)> listFreeSeats = listOfAllSeats
                  .Except(listTakenSeats)
                  .ToList();
  */
            //CHECK IF listFreeSeats CONTAINS AT LEAST 1 REQUESTED
            //ROW WHICH HAS EQUAL OR MORE FREE CONTINIOUS SEATS
            //COMPARED TO NUMBER OF REQUESTED SEATS. IF SO, THROW ERROR
            //BECAUSE CLIENT COULD RESERVE ALL SEATS IN THE SAME ROW.
            // IF NOT, ALLOW CLIENT SEPARATE RESERVATIONS.



            //LASTLY, CHECK IF SEATS ARE NEXT TO EACH OTHER.
            //IF THEY ARE, PROCEED. IF NOT, CHECK IF ROW
            // CONTAINS ENOUGH CONTINIOUS SEATS. IF IT DOES, 
            // THROW ERROR BECAUSE CLIENT COULD RESERVE PROPERLY.
            // IF NOT, ALLOW SEPARATE RESERVATIONS IN SAME ROW.
            #endregion

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

            if(reservations.Count() < 1)
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
