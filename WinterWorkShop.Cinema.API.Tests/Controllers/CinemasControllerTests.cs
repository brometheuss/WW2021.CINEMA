using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class CinemasControllerTests
    {
        private Mock<ICinemaService> _cinemaService;

        [TestMethod]
        public void GetAsync_Return_NewEmptyList()
        {
            //Arrange
            IEnumerable<CinemaDomainModel> cinemaDomainModels = null;
            Task<IEnumerable<CinemaDomainModel>> responseTask = Task.FromResult(cinemaDomainModels);
            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultListObjects = ((OkObjectResult)result).Value;
            var cinemaDomainModelResultList = (List<CinemaDomainModel>)resultListObjects;

            //Assert
            Assert.IsNotNull(cinemaDomainModelResultList);
            Assert.AreEqual(expectedResultCount, cinemaDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_All_Cinemas()
        {
            //Arrange
            List<CinemaDomainModel> cinemaDomainModelsList = new List<CinemaDomainModel>();
            CinemaDomainModel cinemaDomainModel = new CinemaDomainModel
            {
                Id = 123,
                Name = "Bioskop123",
                CityId = 23,
                AuditoriumsList = new List<AuditoriumDomainModel>()
            };

            cinemaDomainModelsList.Add(cinemaDomainModel);

            IEnumerable<CinemaDomainModel> cinemaDomainModels = cinemaDomainModelsList;
            Task<IEnumerable<CinemaDomainModel>> responseTask = Task.FromResult(cinemaDomainModels);

            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.GetAllAsync()).Returns(responseTask);

            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultListObjects = ((OkObjectResult)result).Value;
            var cinemaDomainModelResultList = (List<CinemaDomainModel>)resultListObjects;

            //Assert
            Assert.IsNotNull(cinemaDomainModelResultList);
            Assert.AreEqual(expectedResultCount, cinemaDomainModelResultList.Count);
            Assert.AreEqual(cinemaDomainModel.Id, cinemaDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_Create_IsSuccessful_True_Cinema()
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateCinemaModel createCinemaModel = new CreateCinemaModel()
            {
                Name = "bioskop123",
                CityId = 123,
                SeatRows = 15,
                NumberOfSeats = 11
            };

            CinemaDomainModel cinemaDomainModel = new CinemaDomainModel
            {
                CityId = createCinemaModel.CityId,
                Name = createCinemaModel.Name,
                AuditoriumsList = new List<AuditoriumDomainModel>()
            };

            Task<CinemaDomainModel> responseTask = Task.FromResult(cinemaDomainModel);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.CreateCinemaAsync(It.IsAny<CinemaDomainModel>(), It.IsAny<int>(), It.IsAny<int>())).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);


            //Act
            var result = cinemasController.PostAsync(createCinemaModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var cinemaReturnedModel = (CinemaDomainModel)createdResult;

            //Assert
            Assert.IsNotNull(cinemaReturnedModel);
            Assert.AreEqual(createCinemaModel.CityId, cinemaReturnedModel.CityId);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_CreateCinema_Throw_DbException_Cinema()
        {
            //Arrange 
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateCinemaModel createCinemaModel = new CreateCinemaModel
            {
                Name = "Bioskop12345",
                CityId = 333,
                NumberOfSeats = 12,
                SeatRows = 12
            };

            CinemaDomainModel cinemaDomainModel = new CinemaDomainModel
            {
                Id = 123,
                CityId = createCinemaModel.CityId,
                Name = createCinemaModel.Name,
                AuditoriumsList = new List<AuditoriumDomainModel>()
            };

            Task<CinemaDomainModel> responseTask = Task.FromResult(cinemaDomainModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.CreateCinemaAsync(It.IsAny<CinemaDomainModel>(), It.IsAny<int>(), It.IsAny<int>())).Throws(dbUpdateException);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.PostAsync(createCinemaModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(errorResult);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_Create_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = Messages.CINEMA_CREATION_ERROR;
            int expectedStatusCode = 400;

            CreateCinemaModel createCinemaModel = new CreateCinemaModel
            {
                Name = "bioskop",
                CityId = 23,
                NumberOfSeats = 13,
                SeatRows = 13
            };

            CinemaDomainModel cinemaDomainModel = null;

            Task<CinemaDomainModel> responseTask = Task.FromResult(cinemaDomainModel);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.CreateCinemaAsync(It.IsAny<CinemaDomainModel>(), It.IsAny<int>(), It.IsAny<int>())).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.PostAsync(createCinemaModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(errorResult);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_With_Invalid_ModelState_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateCinemaModel createCinemaModel = new CreateCinemaModel
            {
                Name = "bioskop",
                CityId = 23,
                NumberOfSeats = 13,
                SeatRows = 13
            };

            _cinemaService = new Mock<ICinemaService>();
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);
            cinemasController.ModelState.AddModelError("key", "Invalid Model State");


            //Act
            var result = cinemasController.PostAsync(createCinemaModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var createdResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = ((SerializableError)createdResult).GetValueOrDefault("key");
            var message = (string[])errorResponse;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);

        }

        [TestMethod]
        public void DeleteAsync_DeleteCinema_IsSuccessful()
        {
            //Arrange
            int cinemaDeleteId = 12;
            int expectedStatusCode = 202;

            CinemaDomainModel cinemaDomainModel = new CinemaDomainModel
            {
                Id = cinemaDeleteId,
                CityId = 1234,
                Name = "Bioskop",
                AuditoriumsList = new List<AuditoriumDomainModel>()
            };

            Task<CinemaDomainModel> responseTask = Task.FromResult(cinemaDomainModel);


            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.DeleteCinemaAsync(It.IsAny<int>())).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.DeleteAsync(cinemaDeleteId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((AcceptedResult)result).Value;
            CinemaDomainModel cinemaDomainModelResult = (CinemaDomainModel)objectResult;


            //Assert
            Assert.IsNotNull(cinemaDomainModel);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
            Assert.AreEqual(cinemaDeleteId, cinemaDomainModelResult.Id);

        }
        [TestMethod]
        public void DeleteAsync_DeleteCinema_Failed_Throw_DbException()
        {
            //Arrange 
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;


            CinemaDomainModel cinemaDomainModel = new CinemaDomainModel
            {
                Id = 123,
                CityId = 555,
                Name = "ime",
                AuditoriumsList = new List<AuditoriumDomainModel>()
            };

            Task<CinemaDomainModel> responseTask = Task.FromResult(cinemaDomainModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.DeleteCinemaAsync(It.IsAny<int>())).Throws(dbUpdateException);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.DeleteAsync(cinemaDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(errorResult);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);

        }
        [TestMethod]
        public void DeleteAsync_DeleteCinema_Failed_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = Messages.CINEMA_DOES_NOT_EXIST;
            int expectedStatusCode = 400;

        
            CinemaDomainModel cinemaDomainModel = null;

            Task<CinemaDomainModel> responseTask = Task.FromResult(cinemaDomainModel);

            _cinemaService = new Mock<ICinemaService>();
            _cinemaService.Setup(x => x.DeleteCinemaAsync(It.IsAny<int>())).Returns(responseTask);
            CinemasController cinemasController = new CinemasController(_cinemaService.Object);

            //Act
            var result = cinemasController.DeleteAsync(123).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(errorResult);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
        }
    }
}
