using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;
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
        private Reservation _reservation;
        private ReservationDomainModel _reservationDomainModel;
        private ReservationService _reservationService;

        [TestInitialize]
        public void TestInitialize()
        {
            _reservation = new Reservation
            {
                Id = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
            };

            _mockReservationRepository = new Mock<IReservationsRepository>();
            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockProjectionRepository = new Mock<IProjectionsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();

            _reservationService = new ReservationService(_mockReservationRepository.Object, _mockSeatRepository.Object, _mockProjectionRepository.Object, _mockAuditoriumRepository.Object);
        }
    }
}
