using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class CitiesControllerTests
    {
        private Mock<ICityService> _cityService;
        [TestMethod]
        public void GetAsync_Return_All_Cities()
        {
            //Arrange
            CityDomainModel cityDomainModel = new CityDomainModel
            {
                Id = 123,
                CinemasList = new List<CinemaDomainModel>(),
                Name = "Zrenjanin"
            };
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            List<CityDomainModel> cityDomainModels = new List<CityDomainModel>();
            cityDomainModels.Add(cityDomainModel);

            IEnumerable<CityDomainModel> cityDomainModelsIEn = cityDomainModels;

            Task<IEnumerable<CityDomainModel>> responseTask = Task.FromResult(cityDomainModelsIEn);
            _cityService = new Mock<ICityService>();
            _cityService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            CitiesController citiesController = new CitiesController(_cityService.Object);

            //Act
            var result = citiesController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((OkObjectResult)result).Value;
            List<CityDomainModel> cityDomainModelResultList = (List<CityDomainModel>)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(expectedResultCount, cityDomainModelResultList.Count);
            Assert.AreEqual(cityDomainModel.Id, cityDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }

        [TestMethod]
        public void GetAsync_Return_Empty_List()
        {
            //Arrange
            CityDomainModel cityDomainModel = new CityDomainModel
            {
                Id = 123,
                CinemasList = new List<CinemaDomainModel>(),
                Name = "Zrenjanin"
            };
            
            int expectedStatusCode = 200;

            IEnumerable<CityDomainModel> cityDomainModelsIEn = null;

            Task<IEnumerable<CityDomainModel>> responseTask = Task.FromResult(cityDomainModelsIEn);
            _cityService = new Mock<ICityService>();
            _cityService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            CitiesController citiesController = new CitiesController(_cityService.Object);

            //Act
            var result = citiesController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((OkObjectResult)result).Value;
            List<CityDomainModel> cityDomainModelResultList = (List<CityDomainModel>)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetCity_ById_Async_Return_City_OkObjectResult()
        {
            //Arrange
            CityDomainModel cityDomainModel = new CityDomainModel
            {
                Id = 123,
                CinemasList = new List<CinemaDomainModel>(),
                Name = "Zrenjanin"
            };

            int expectedStatusCode = 200;
            Task<CityDomainModel> responseTask = Task.FromResult(cityDomainModel);
            _cityService = new Mock<ICityService>();
            _cityService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(responseTask);
            CitiesController citiesController = new CitiesController(_cityService.Object);

            //Act
            var result = citiesController.GetById(cityDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((OkObjectResult)result).Value;
            CityDomainModel cityDomainResult = (CityDomainModel)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual(cityDomainModel.Id, cityDomainResult.Id);
        }

        [TestMethod]
        public void GetCity_ById_Async_Return_Not_Found()
        {
            //Arrange
            string expectedMessage = Messages.CITY_NOT_FOUND;
            int expectedStatusCode = 404;
            CityDomainModel cityDomainModel = null;
            Task<CityDomainModel> responseTask = Task.FromResult(cityDomainModel);
            _cityService = new Mock<ICityService>();
            _cityService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(responseTask);
            CitiesController citiesController = new CitiesController(_cityService.Object);

            //Act
            var result = citiesController.GetById(123).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((NotFoundObjectResult)result).Value;
            string message = (string)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
            Assert.AreEqual(expectedMessage, message);
        }
    }
}
