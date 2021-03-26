using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class ReservationServiceTests
    {
        private Mock<IReservationsRepository> _mockReservationRepository;
        private Mock<ISeatsRepository> _mockSeatRepository;
        private Mock<IProjectionsRepository> _mockProjectionRepository;
        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private Mock<IReservationService> _mockReservationService;
        private Mock<IUsersRepository> _mockUserRepository;
        private Reservation _reservation;
        private ReservationService _reservationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                ReservationSeats = new List<ReservationSeat>()
            };

            _mockReservationRepository = new Mock<IReservationsRepository>();
            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockProjectionRepository = new Mock<IProjectionsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();
            _mockUserRepository = new Mock<IUsersRepository>();

            _mockReservationService = new Mock<IReservationService>();

            _reservationService = new ReservationService(_mockReservationRepository.Object, _mockSeatRepository.Object, _mockProjectionRepository.Object, _mockAuditoriumRepository.Object, _mockUserRepository.Object);
        }

        //GetAllReservations tests
        [TestMethod]
        public void ReservationService_GetAllReservations_ReturnsListOfReservations()
        {
            //Arrange
            var expectedCount = 2;
            Reservation reservation2 = new Reservation
            {
                Id = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            List<Reservation> reservations = new List<Reservation>();
            reservations.Add(_reservation);
            reservations.Add(reservation2);
            _mockReservationRepository.Setup(x => x.GetAll()).ReturnsAsync(reservations);

            //Act
            var resultAction = _reservationService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(_reservation.Id, result[0].Id);
            Assert.AreEqual(reservation2.Id, result[1].Id);
            Assert.IsInstanceOfType(result[0], typeof(ReservationDomainModel));
        }

        [TestMethod]
        public void ReservationService_GetAllReservations_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<Reservation> reservations = new List<Reservation>();
            _mockReservationRepository.Setup(x => x.GetAll()).ReturnsAsync(reservations);

            //Act
            var resultAction = _reservationService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }

        //GetById tests
        [TestMethod]
        public void ReservationService_GetReservationById_ReturnsReservation()
        {
            //Arrange
            _mockReservationRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_reservation);

            //Act
            var resultAction = _reservationService.GetByIdAsync(_reservation.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_GetReservationById_ReturnsNull()
        {
            //Arrange
            _mockReservationRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Reservation);

            //Act
            var resultAction = _reservationService.GetByIdAsync(It.IsAny<Guid>()).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //GetTakenSeats tests
        [TestMethod]
        public void ReservationService_GetTakenSeats_ReturnsListOfSeats()
        {
            //Arrange
            var expectedSeatCount = 2;

            Seat s1 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 1,
                ReservationSeats = new List<ReservationSeat>()
            };
            Seat s2 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 1,
                ReservationSeats = new List<ReservationSeat>()
            };

            ReservationSeat rs1 = new ReservationSeat
            {
                ReservationId = _reservation.Id,
                SeatId = s1.Id,
                Seat = s1,
                Reservation = _reservation
            };
            ReservationSeat rs2 = new ReservationSeat
            {
                ReservationId = _reservation.Id,
                SeatId = s2.Id,
                Seat = s2,
                Reservation = _reservation
            };

            Projection projection = new Projection
            {
                Id = _reservation.ProjectionId,
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                DateTime = DateTime.Now.AddDays(4)
            };
            List<Reservation> reservations = new List<Reservation>();
            
            List<Seat> seats = new List<Seat>();
            seats.Add(s1);
            seats.Add(s2);
            

            _reservation.ReservationSeats.Add(rs1);
            _reservation.ReservationSeats.Add(rs2);

            s1.ReservationSeats.Add(rs1);
            s2.ReservationSeats.Add(rs2);

            reservations.Add(_reservation);

            _mockReservationRepository.Setup(x => x.GetReservationByProjectionId(It.IsAny<Guid>())).Returns(reservations);

            //Act
            var resultAction = _reservationService.GetTakenSeats(projection.Id);
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedSeatCount, result.Count);
            Assert.AreEqual(result[0].SeatDomainModel.Id, s1.Id);
            Assert.AreEqual(result[1].SeatDomainModel.Id, s2.Id);
            Assert.IsTrue(result[0].IsSuccessful);
            Assert.IsNull(result[0].ErrorMessage);
            Assert.IsInstanceOfType(result[0].SeatDomainModel, typeof(SeatDomainModel));
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<SeatResultModel>));
        }

        [TestMethod]
        public void ReservationService_GetTakenSeats_ReturnsNull()
        {
            //Arrange
            Projection projection = new Projection
            {
                Id = Guid.NewGuid()
            };
            List<Reservation> reservations = new List<Reservation>();
            _mockReservationRepository.Setup(x => x.GetReservationByProjectionId(It.IsAny<Guid>())).Returns(reservations);

            //Act
            var resultAction = _reservationService.GetTakenSeats(projection.Id);

            //Assert
            Assert.IsNull(resultAction);
        }

        //CreateReservation tests
        [TestMethod]
        public void ReservationService_CreateReservation_ReturnsCreatedReservation()
        {
            //Arrange
            
            Projection projection = new Projection
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                DateTime = DateTime.Now.AddHours(5),
                MovieId = Guid.NewGuid()
            };
            SeatResultModel srm1 = new SeatResultModel
            {
                ErrorMessage = null,
                IsSuccessful = true,
                SeatDomainModel = new SeatDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = 1,
                    Number = 1,
                    Row = 1
                }
            };
            SeatResultModel srm2 = new SeatResultModel
            {
                ErrorMessage = null,
                IsSuccessful = true,
                SeatDomainModel = new SeatDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumId = 1,
                    Number = 2,
                    Row = 1
                }
            };
            List<SeatResultModel> seatResults = new List<SeatResultModel>();

            Seat s1 = new Seat
            {
                Id = srm1.SeatDomainModel.Id,
                AuditoriumId = srm1.SeatDomainModel.AuditoriumId,
                Number = srm1.SeatDomainModel.Number,
                Row = srm1.SeatDomainModel.Row
            }; 
            Seat s2 = new Seat
            {
                Id = srm2.SeatDomainModel.Id,
                AuditoriumId = srm2.SeatDomainModel.AuditoriumId,
                Number = srm2.SeatDomainModel.Number,
                Row = srm2.SeatDomainModel.Row
            };
            List<Seat> seats = new List<Seat>();
            seats.Add(s1);
            seats.Add(s2);
            CreateReservationModel reservation = new CreateReservationModel
            {
                ProjectionId = projection.Id,
                UserId = Guid.NewGuid(),
                SeatIds = seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                })
            };
            //TO BE CONTINUED.
            
            Reservation reservation2 = new Reservation
            {
                Id = Guid.NewGuid(),
                ProjectionId = reservation.ProjectionId,
                UserId = reservation.UserId,
                ReservationSeats = new List<ReservationSeat>()
            };
            ReservationSeat rs1 = new ReservationSeat
            {
                SeatId = s1.Id,
                Seat = s1,
                ReservationId = Guid.NewGuid(),
                Reservation = reservation2
            };
            reservation2.ReservationSeats.Add(rs1);

            _mockReservationService.Setup(x => x.GetTakenSeats(It.IsAny<Guid>())).Returns(seatResults);
            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seats);
            _mockProjectionRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(projection);
            _mockSeatRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(s1);
            _mockReservationRepository.Setup(x => x.Insert(It.IsAny<Reservation>())).Returns(reservation2);

            //Assert
            var resultAction = _reservationService.CreateReservation(reservation);

            //Act
            Assert.IsNotNull(resultAction);
        }

        [TestMethod]
        public void ReservationService_CreateReservation_ReturnsErrorMessage_ProjectionNotExist()
        {
            //Arrange
            List<SeatResultModel> seatResultList = new List<SeatResultModel>();
            List<Seat> seats = new List<Seat>();
            Seat s1 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 1
            };
            Seat s2 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 1
            };
            seats.Add(s1);
            seats.Add(s2);
            CreateReservationModel reservation = new CreateReservationModel()
            {
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                SeatIds = seats.Select(s => new SeatDomainModel
                {
                    Id = s.Id,
                    AuditoriumId = s.AuditoriumId,
                    Number = s.Number,
                    Row = s.Row
                })
            };

            _mockReservationService.Setup(x => x.GetTakenSeats(It.IsAny<Guid>())).Returns(seatResultList);
            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seats);
            _mockProjectionRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Projection);

            //Act
            var resultAction = _reservationService.CreateReservation(reservation);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.AreEqual(Messages.PROJECTION_DOES_NOT_EXIST, resultAction.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(ReservationResultModel));
        }

        [TestMethod]
        public void ReservationService_CreateReservation_ReturnsErrorMessage_SeatNotExist()
        {
            //Arrange
            List<SeatDomainModel> seatDomainModels = new List<SeatDomainModel>();
            SeatDomainModel seat1 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 1
            };
            SeatDomainModel seat2 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 3,
                Row = 1
            };
            seatDomainModels.Add(seat1);
            seatDomainModels.Add(seat2);
            List<SeatResultModel> seatsTaken = new List<SeatResultModel>();
            Seat s1 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 4,
                Row = 1
            };
            Seat s2 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 5,
                Row = 1
            };
            List<Seat> seats = new List<Seat>();
            seats.Add(s1);
            seats.Add(s2);
            CreateReservationModel createReservationModel = new CreateReservationModel()
            {
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                SeatIds = new List<SeatDomainModel>()
            };
            Projection projection = new Projection
            {
                Id = createReservationModel.ProjectionId,
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                DateTime = DateTime.Now.AddDays(1)
            };
            createReservationModel.SeatIds = seatDomainModels;
            _mockReservationService.Setup(x => x.GetTakenSeats(createReservationModel.ProjectionId)).Returns(seatsTaken);
            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seats);
            _mockProjectionRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(projection);

            //Act
            var resultAction = _reservationService.CreateReservation(createReservationModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(Messages.SEAT_SEATS_NOT_EXIST_FOR_AUDITORIUM, resultAction.ErrorMessage);
            Assert.IsNull(resultAction.Reservation);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsInstanceOfType(resultAction, typeof(ReservationResultModel));
        }

        [TestMethod]
        public void ReservationService_CreateReservation_ReturnsErrorMessage_SeatNotInTheSameRow()
        {
            //Arrange
            List<SeatDomainModel> seatsRequested = new List<SeatDomainModel>();
            SeatDomainModel seat1 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 1
            };
            SeatDomainModel seat2 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 3,
                Row = 1
            };
            SeatDomainModel seat3 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 2
            };
            seatsRequested.Add(seat1);
            seatsRequested.Add(seat2);
            seatsRequested.Add(seat3);
            List<SeatResultModel> seatsTaken = new List<SeatResultModel>();
            Seat s1 = new Seat
            {
                Id = seat1.Id,
                AuditoriumId = seat1.AuditoriumId,
                Number = seat1.Number,
                Row = seat1.Row
            };
            Seat s2 = new Seat
            {
                Id = seat2.Id,
                AuditoriumId = seat2.AuditoriumId,
                Number = seat2.Number,
                Row = seat2.Row
            };
            Seat s3 = new Seat
            {
                Id = seat3.Id,
                AuditoriumId = seat3.AuditoriumId,
                Number = seat3.Number,
                Row = seat3.Row
            };
            List<Seat> seatsInAuditorium = new List<Seat>();
            seatsInAuditorium.Add(s1);
            seatsInAuditorium.Add(s2);
            seatsInAuditorium.Add(s3);
            CreateReservationModel createReservationModel = new CreateReservationModel()
            {
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                SeatIds = new List<SeatDomainModel>()
            };
            Projection projection = new Projection
            {
                Id = createReservationModel.ProjectionId,
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                DateTime = DateTime.Now.AddDays(1)
            };
            createReservationModel.SeatIds = seatsRequested;
            _mockReservationService.Setup(x => x.GetTakenSeats(createReservationModel.ProjectionId)).Returns(seatsTaken);
            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seatsInAuditorium);
            _mockProjectionRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(projection);
            _mockSeatRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(s1);

            //Act
            var resultAction = _reservationService.CreateReservation(createReservationModel);

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(Messages.SEAT_SEATS_CANNOT_BE_DUPLICATES, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsNull(resultAction.Reservation);
            Assert.IsInstanceOfType(resultAction, typeof(ReservationResultModel));
        }
    }
}
