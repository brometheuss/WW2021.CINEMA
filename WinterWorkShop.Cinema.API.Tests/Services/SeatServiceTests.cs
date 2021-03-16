using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class SeatServiceTests
    {
        private Mock<ISeatsRepository> _mockSeatRepository;
        private SeatService _seatService;
        private Seat _seat;
        private SeatDomainModel _seatDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _seat = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 1
            };

            _seatDomainModel = new SeatDomainModel
            {
                Id = _seat.Id,
                AuditoriumId = _seat.AuditoriumId,
                Number = _seat.Number,
                Row = _seat.Row
            };

            _mockSeatRepository = new Mock<ISeatsRepository>();
            _seatService = new SeatService(_mockSeatRepository.Object);
        }

        [TestMethod]
        public void SeatService_GetAllSeats_ReturnsListOfSeats()
        {
            //Arrange
            var expectedCount = 2;

            Seat seat2 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 1
            };
            SeatDomainModel seatDomainModel2 = new SeatDomainModel
            {
                Id = seat2.Id,
                AuditoriumId = seat2.AuditoriumId,
                Number = seat2.Number,
                Row = seat2.Row
            };
            List<Seat> seats = new List<Seat>();
            seats.Add(_seat);
            seats.Add(seat2);

            List<SeatDomainModel> seatDomainModels = new List<SeatDomainModel>();
            seatDomainModels.Add(_seatDomainModel);
            seatDomainModels.Add(seatDomainModel2);

            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seats);

            //Act
            var resultAction = _seatService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(_seat.Id, result[0].Id);
            Assert.AreEqual(seatDomainModel2.Id, result[1].Id);
            Assert.IsInstanceOfType(result[0], typeof(SeatDomainModel));
        }

        [TestMethod]
        public void SeatService_GetAllSeats_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;

            List<Seat> seats = new List<Seat>();
            List<SeatDomainModel> seatDomainModels = new List<SeatDomainModel>();

            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seats);

            //Act
            var resultAction = _seatService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
        }
    }
}
