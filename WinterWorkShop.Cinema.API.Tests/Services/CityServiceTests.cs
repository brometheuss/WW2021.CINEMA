using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class CityServiceTests
    {
        private Mock<ICitiesRepository> _cityRepository;
        private City _city;
        private CityDomainModel _cityDomainModel;
        private CityService _cityService;
        private Data.Cinema _cinema;

        [TestInitialize]
        public void TestInitialize()
        {

            _city = new City
            {
                Id = 1,
                Name = "Miami",
                Cinemas = new List<Data.Cinema>()
            };
            _cinema = new Data.Cinema
            {
                Id = 3,
                CityId = 1,
                Name = "Veoma los bioskop"
            };
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Add(_cinema);
            _city.Cinemas = cinemas;

            _cityRepository = new Mock<ICitiesRepository>();
            _cityService = new CityService(_cityRepository.Object);
        }

        //GetAllCities tests
        [TestMethod]
        public void CityService_GetAllAsync_ReturnsListOfCities()
        {
            //Arrange
            var expectedCount = 2;
            var cinemaCount = 1;
            City city2 = new City
            {
                Id = 2,
                Name = "Rome",
                Cinemas = new List<Data.Cinema>()
            };
            Data.Cinema cinema2 = new Data.Cinema
            {
                Id = 2,
                CityId = 2,
                Name = "Smekerski bioskop"
            };
            
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Add(cinema2);
            city2.Cinemas = cinemas;
            List<City> cities = new List<City>();
            cities.Add(city2);
            cities.Add(city2);
            _cityRepository.Setup(x => x.GetAll()).ReturnsAsync(cities);

            //Act
            var resultAction = _cityService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(city2.Id, result[0].Id);
            Assert.IsInstanceOfType(result[0], typeof(CityDomainModel));
            Assert.AreEqual(cinemaCount, result[0].CinemasList.Count);
        }

        [TestMethod]
        public void CityService_GetAllAsync_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<City> cities = new List<City>();
            _cityRepository.Setup(x => x.GetAll()).ReturnsAsync(cities);

            //Act
            var resultAction = _cityService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, resultAction.Count());
        }

        //GetCityById tests
        [TestMethod]
        public void CityService_GetByIdAsync_ReturnsCity()
        {
            //Arrange
            _city = new City
            {
                Id = 1,
                Name = "sigh",
                Cinemas = new List<Data.Cinema>()
            };
            Data.Cinema cinema2 = new Data.Cinema
            {
                Id = 1,
                CityId = 1,
                Name = "prolecni bioskop"
            };
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Add(cinema2);
            _city.Cinemas = cinemas;
            _cityRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_city);

            //Act
            var resultAction = _cityService.GetByIdAsync(_city.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_city.Id, resultAction.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CityDomainModel));
        }

        [TestMethod]
        public void CityService_GetByIdAsync_ReturnsNull()
        {
            //Arrange
            _cityRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as City);

            //Act
            var resultAction = _cityService.GetByIdAsync(_city.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void CityService_GetByIdAsync_ReturnsCity_WithEmptyListOfCinemas()
        {
            //Arrange
            var expectedCount = 0;
            _city = new City
            {
                Id = 100,
                Name = "Los santos",
                Cinemas = new List<Data.Cinema>()
            };
            _cityRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_city);

            //Act
            var resultAction = _cityService.GetByIdAsync(_city.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, resultAction.CinemasList.Count);
            Assert.AreEqual(_city.Id, resultAction.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CityDomainModel));
        }

        //GetCityByCityName tests
        [TestMethod]
        public void CityService_GetByCityNameAsync_ReturnsCity()
        {
            //Arrange
            var expectedCount = 0;
            _city = new City
            {
                Id = 99,
                Name = "Grad koji trazimo",
                Cinemas = new List<Data.Cinema>()
            };
            _cityRepository.Setup(x => x.GetByCityNameAsync(It.IsAny<string>())).ReturnsAsync(_city);

            //Act
            var resultAction = _cityService.GetByCityNameAsync(_city.Name).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_city.Name, resultAction.Name);
            Assert.AreEqual(_city.Cinemas.Count, resultAction.CinemasList.Count);
            Assert.IsInstanceOfType(resultAction, typeof(CityDomainModel));
        }

        [TestMethod]
        public void CityService_GetByCityNameAsync_ReturnsNull()
        {
            //Arrange
            _cityRepository.Setup(x => x.GetByCityNameAsync(It.IsAny<string>())).ReturnsAsync(null as City);

            //Act
            var resultAction = _cityService.GetByCityNameAsync(_city.Name).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }
    }
}
