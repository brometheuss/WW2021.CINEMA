using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void CinemaService_GetAllCinemas_ReturnsListOfCinemas()
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
        public void CinemaService_GetAllCinemas_ReturnsEmptyList()
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
    }
}
