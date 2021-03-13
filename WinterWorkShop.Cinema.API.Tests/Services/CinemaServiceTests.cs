﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Data;
using Moq;
using WinterWorkShop.Cinema.Repositories;
using WinterWorkShop.Cinema.Domain.Services;
using System.Linq;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class CinemaServiceTests
    {
        private Mock<ICinemasRepository> _mockCinemaRepository;
        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private Mock<ISeatsRepository> _mockSeatRepository;
        private Mock<ICitiesRepository> _mockCityRepository;
        private Data.Cinema _cinema;
        private CinemaDomainModel _cinemaDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _cinema = new Data.Cinema
            {
                Id = 11,
                CityId = 1,
                Name = "Smekerski bioskop"
            };

            _cinemaDomainModel = new CinemaDomainModel
            {
                Id = _cinema.Id,
                CityId = _cinema.CityId,
                Name = _cinema.Name
            };

            _mockCinemaRepository = new Mock<ICinemasRepository>();
            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();
            _mockCityRepository = new Mock<ICitiesRepository>();
        }

        //GetAllCinemas tests
        [TestMethod]
        public void CinemaService_GetAllCinemasAsync_ReturnsListOfCinemas()
        {
            //Arrange
            var expectedCount = 2;
            Data.Cinema cinema2 = new Data.Cinema
            {
                Id = 22,
                CityId = 1,
                Name = "Los bioskop"
            };
            CinemaDomainModel cinemaDomainModel2 = new CinemaDomainModel
            {
                Id = cinema2.Id,
                CityId = cinema2.CityId,
                Name = cinema2.Name
            };
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Add(_cinema);
            cinemas.Add(cinema2);
            _mockCinemaRepository.Setup(x => x.GetAll()).ReturnsAsync(cinemas);
            CinemaService cinemaService = new CinemaService(_mockCinemaRepository.Object, _mockAuditoriumRepository.Object, _mockSeatRepository.Object, _mockCityRepository.Object);

            //Act
            var resultAction = cinemaService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(_cinema.Name, result[0].Name);
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<CinemaDomainModel>));
        }

        [TestMethod]
        public void CinemaService_GetAllCinemasAsync_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Clear();
            _mockCinemaRepository.Setup(x => x.GetAll()).ReturnsAsync(cinemas);
            CinemaService cinemaService = new CinemaService(_mockCinemaRepository.Object, _mockAuditoriumRepository.Object, _mockSeatRepository.Object, _mockCityRepository.Object);

            //Act
            var resultAction = cinemaService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(resultAction, typeof(List<CinemaDomainModel>));
        }
        
        //DeleteCinema tests
        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsDeletedCinema()
        {
            //Arrange
            List<Seat> seats1 = new List<Seat>();
            List<Seat> seats2 = new List<Seat>();
            var seat1 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 90,
                Row = 10,
                AuditoriumId = 2                
            };
            var seat2 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 91,
                Row = 10,
                AuditoriumId = 2
            };
            var seat3 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 92,
                Row = 10,
                AuditoriumId = 3
            };
            seats1.Add(seat1);
            seats1.Add(seat2);
            seats2.Add(seat3);
          
            Auditorium audit1 = new Auditorium
            {
                Id = 2,
                AuditName = "Auditorium 1",
                CinemaId = 11,
                Seats = seats1
            };

            Auditorium audit2 = new Auditorium
            {
                Id = 3,
                AuditName = "Auditorium 2",
                CinemaId = 11,
                Seats = seats2
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);
            auditsList.Add(audit2);

            _mockCinemaRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_cinema);
            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditsList);
            CinemaService cinemaService = new CinemaService(_mockCinemaRepository.Object, _mockAuditoriumRepository.Object, _mockSeatRepository.Object, _mockCityRepository.Object);

            //Act
            var resultAction = cinemaService.DeleteCinemaAsync(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            
            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_cinema.Id, resultAction.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CinemaDomainModel));
        }

        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsNullCinema()
        {
            //Arrange
            Auditorium audit1 = new Auditorium
            {
                Id = 2,
                AuditName = "Auditorium 1",
                CinemaId = 11,
            };

            Auditorium audit2 = new Auditorium
            {
                Id = 3,
                AuditName = "Auditorium 2",
                CinemaId = 11,
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);
            auditsList.Add(audit2);


            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Data.Cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditsList);
            _mockCinemaRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_cinema);
            CinemaService cinemaService = new CinemaService(_mockCinemaRepository.Object, _mockAuditoriumRepository.Object, _mockSeatRepository.Object, _mockCityRepository.Object);

            //Act
            var resultAction = cinemaService.DeleteCinemaAsync(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsEmptyListAuditoriums()
        {
            //Arrange
            List<Seat> seats1 = new List<Seat>();
            List<Seat> seats2 = new List<Seat>();
            var seat1 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 90,
                Row = 10,
                AuditoriumId = 2
            };
            var seat2 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 91,
                Row = 10,
                AuditoriumId = 2
            };
            var seat3 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 92,
                Row = 10,
                AuditoriumId = 2
            };
            seats1.Add(seat1);
            seats1.Add(seat2);
            seats1.Add(seat3);

            Auditorium audit1 = new Auditorium
            {
                Id = 2,
                AuditName = "Auditorium 1",
                CinemaId = 11,
                Seats = seats1
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);

            var expectedCount = 0;
            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seats1);
            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditsList);
            CinemaService cinemaService = new CinemaService(_mockCinemaRepository.Object, _mockAuditoriumRepository.Object, _mockSeatRepository.Object, _mockCityRepository.Object);

            //Act
            var resultAction = cinemaService.DeleteCinemaAsync(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.AuditoriumsList.Count();

            //Assert
            Assert.AreEqual(expectedCount, result);
        }
    }
}
