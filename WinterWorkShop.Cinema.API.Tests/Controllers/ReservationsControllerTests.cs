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
    public class ReservationsControllerTests
    {
        private Mock<IReservationService> _reservationService;

        [TestMethod]
        public void GetAsync_Return_All_Reservations()
        {
            //Arrange
            ReservationDomainModel reservationDomainModel = new ReservationDomainModel
            {
                Id = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };
            int expectedStatusCode = 200;
            int expectedResultCount = 1;

            List<ReservationDomainModel> reservationDomainModels = new List<ReservationDomainModel>();
            reservationDomainModels.Add(reservationDomainModel);

            IEnumerable<ReservationDomainModel> reservationsIEn = reservationDomainModels;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservationsIEn);
            _reservationService = new Mock<IReservationService>();
            _reservationService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            ReservationsController reservationsController = new ReservationsController(_reservationService.Object);

            //Act
            var result = reservationsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<ReservationDomainModel> reservationDomainModelsResult = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(reservationDomainModelsResult);
            Assert.AreEqual(expectedResultCount, reservationDomainModelsResult.Count);
            Assert.AreEqual(reservationDomainModel.Id, reservationDomainModelsResult[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_Empty_Reservation_List()
        {
            //Arrange
            
            int expectedStatusCode = 200;
  
            IEnumerable<ReservationDomainModel> reservationsIEn = null;
            Task<IEnumerable<ReservationDomainModel>> responseTask = Task.FromResult(reservationsIEn);
            _reservationService = new Mock<IReservationService>();
            _reservationService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            ReservationsController reservationsController = new ReservationsController(_reservationService.Object);

            //Act
            var result = reservationsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<ReservationDomainModel> reservationDomainModelsResult = (List<ReservationDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(reservationDomainModelsResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetById_Return_Reservation_Ok_Object_Result()
        {
            //Arrange
            ReservationDomainModel reservationDomainModel = new ReservationDomainModel
            {
                Id = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                UserId = Guid.NewGuid()
            };

            int expectedStatusCode = 200;
            Task<ReservationDomainModel> responseTask = Task.FromResult(reservationDomainModel);
            _reservationService = new Mock<IReservationService>();
            _reservationService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            ReservationsController reservationsController = new ReservationsController(_reservationService.Object);

            //Act
            var result = reservationsController.GetById(reservationDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((OkObjectResult)result).Value;
            ReservationDomainModel reservationDomainResult = (ReservationDomainModel)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
            Assert.AreEqual(reservationDomainModel.Id, reservationDomainResult.Id);
        }
        [TestMethod]
        public void GetById_Return_NotFound()
        {
            //Arrange
            ReservationDomainModel reservationDomainModel = null;
            string expectedMessage = Messages.RESERVATION_NOT_FOUND;

            int expectedStatusCode = 404;
            Task<ReservationDomainModel> responseTask = Task.FromResult(reservationDomainModel);
            _reservationService = new Mock<IReservationService>();
            _reservationService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            ReservationsController reservationsController = new ReservationsController(_reservationService.Object);

            //Act
            var result = reservationsController.GetById(Guid.NewGuid()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((NotFoundObjectResult)result).Value;
            string errorMessage = (string)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
            Assert.AreEqual(expectedMessage, errorMessage);
        }
        [TestMethod]
        public void MakeReservation_ReservationCreatedSuccesful_ReturnCreated()
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateReservationModel createReservationModel = new CreateReservationModel
            {
                UserId = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                SeatIds = new List<SeatDomainModel>()
            };

            ReservationResultModel reservationResultModel = new ReservationResultModel
            {
                Reservation = new ReservationDomainModel
                {
                    Id = Guid.NewGuid(),
                    ProjectionId = Guid.NewGuid(),
                    UserId = createReservationModel.UserId
                },
                ErrorMessage = null,
                IsSuccessful = true
            };
            

            _reservationService = new Mock<IReservationService>();
            _reservationService.Setup(x => x
            .CreateReservation(It.IsAny<CreateReservationModel>())).Returns(reservationResultModel);

            ReservationsController reservationsController = new ReservationsController(_reservationService.Object);

            //Act
            var result = reservationsController.MakeReservation(createReservationModel).Result;
            var createdResult = ((CreatedResult)result).Value;
            var reservationResultDomainModel = (ReservationResultModel)createdResult;

            //Assert
            Assert.IsNotNull(createdResult);
            Assert.AreEqual(createReservationModel.UserId, reservationResultDomainModel.Reservation.UserId);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
        }
        [TestMethod]
        public void MakeReservation_Throw_DbUpdateException()
        {
            //Arrange
            int expectedStatusCode = 400;
            string expectedMessage = "Inner exception error message.";

            CreateReservationModel createReservationModel = new CreateReservationModel
            {
                UserId = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                SeatIds = new List<SeatDomainModel>()
            };

            ReservationResultModel reservationResultModel = new ReservationResultModel
            {
                Reservation = new ReservationDomainModel
                {
                    Id = Guid.NewGuid(),
                    ProjectionId = Guid.NewGuid(),
                    UserId = createReservationModel.UserId
                },
                ErrorMessage = null,
                IsSuccessful = true
            };

            
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);


            _reservationService = new Mock<IReservationService>();
            _reservationService.Setup(x => x
            .CreateReservation(It.IsAny<CreateReservationModel>())).Throws(dbUpdateException);

            ReservationsController reservationsController = new ReservationsController(_reservationService.Object);

            //Act
            var result = reservationsController.MakeReservation(createReservationModel).Result;
            var badRequestResultValue = ((BadRequestObjectResult)result).Value;
            var errorResponse = (ErrorResponseModel)badRequestResultValue;

            //Assert
            Assert.IsNotNull(badRequestResultValue);
            Assert.AreEqual(expectedMessage, errorResponse.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void MakeReservation_Reservation_Not_Created_Return_BadRequest()
        {
            //Arrange
            int expectedStatusCode = 400;

            CreateReservationModel createReservationModel = new CreateReservationModel
            {
                UserId = Guid.NewGuid(),
                ProjectionId = Guid.NewGuid(),
                SeatIds = new List<SeatDomainModel>()
            };

            ReservationResultModel reservationResultModel = new ReservationResultModel
            {
                ErrorMessage = null,
                IsSuccessful = false
            };


            _reservationService = new Mock<IReservationService>();
            _reservationService.Setup(x => x
            .CreateReservation(It.IsAny<CreateReservationModel>())).Returns(reservationResultModel);

            ReservationsController reservationsController = new ReservationsController(_reservationService.Object);

            //Act
            var result = reservationsController.MakeReservation(createReservationModel).Result;
            var createdResult = ((BadRequestObjectResult)result).Value;
            var reservationResultDomainModel = (ErrorResponseModel)createdResult;

            //Assert
            Assert.IsNotNull(createdResult);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
        }
    }
}
