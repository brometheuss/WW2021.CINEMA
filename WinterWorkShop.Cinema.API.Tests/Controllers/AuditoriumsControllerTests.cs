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
    public class AuditoriumsControllerTests
    {
        private Mock<IAuditoriumService> _auditoriumService;

        [TestMethod]
        public void GetAsync_Return_All_Auditoriums()
        {
            //Arrange
            List<AuditoriumDomainModel> auditoriumsDomainModelsList = new List<AuditoriumDomainModel>();

            AuditoriumDomainModel auditoriumDomainModel = new AuditoriumDomainModel
            {
                Id = 1,
                Name = "Auditorium1",
                CinemaId = 5,
                SeatsList = new List<SeatDomainModel>()
            };

            auditoriumsDomainModelsList.Add(auditoriumDomainModel);
            IEnumerable<AuditoriumDomainModel> auditoriumDomainModels = auditoriumsDomainModelsList;

            Task<IEnumerable<AuditoriumDomainModel>> responseTask = Task
                .FromResult(auditoriumDomainModels);

            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            //Act
            var result = auditoriumsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var auditoriumDomainModelResultList = (List<AuditoriumDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(auditoriumDomainModelResultList);
            Assert.AreEqual(expectedResultCount, auditoriumDomainModelResultList.Count);
            Assert.AreEqual(auditoriumDomainModel.Id, auditoriumDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_New_List()
        {
            //Arrange
            IEnumerable<AuditoriumDomainModel> auditoriumDomainModels = null;
            Task<IEnumerable<AuditoriumDomainModel>> responseTask = Task.FromResult(auditoriumDomainModels);

            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            //Act
            var result = auditoriumsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var auditoriumDomainModelResultList = (List<AuditoriumDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(auditoriumDomainModelResultList);
            Assert.AreEqual(expectedResultCount, auditoriumDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void PostAsync_Create_createAuditoriumResultModel_IsSuccessful_True_Auditorium()
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel
            {
                auditName = "Auditorium1",
                cinemaId = 23,
                numberOfSeats = 5,
                seatRows = 4
            };

            CreateAuditoriumResultModel createAuditoriumResultModel = new CreateAuditoriumResultModel
            {
                Auditorium = new AuditoriumDomainModel
                {
                    Id = 15,
                    CinemaId = createAuditoriumModel.cinemaId,
                    Name = createAuditoriumModel.auditName,
                    SeatsList = new List<SeatDomainModel>()
                },
                IsSuccessful = true
            };
            Task<CreateAuditoriumResultModel> responseTask = Task.FromResult(createAuditoriumResultModel);

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x
            .CreateAuditorium(It.IsAny<AuditoriumDomainModel>(), It.IsAny<int>(), It.IsAny<int>())).Returns(responseTask);

            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            //Act
            var result = auditoriumsController.PostAsync(createAuditoriumModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var auditoriumDomainModel = (AuditoriumDomainModel)createdResult;

            //Assert
            Assert.IsNotNull(auditoriumDomainModel);
            Assert.AreEqual(createAuditoriumModel.cinemaId, auditoriumDomainModel.CinemaId);
            Assert.AreEqual(createAuditoriumModel.auditName, auditoriumDomainModel.Name);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);


        }

        [TestMethod]
        public void PostAsync_Create_Throw_DbException_Auditorium()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel
            {
                auditName = "Auditorium55",
                cinemaId = 24,
                numberOfSeats = 5,
                seatRows = 6
            };

            CreateAuditoriumResultModel createAuditoriumResultModel = new CreateAuditoriumResultModel
            {
                Auditorium = new AuditoriumDomainModel
                {
                    Id = 25,
                    Name = createAuditoriumModel.auditName,
                    CinemaId = createAuditoriumModel.cinemaId,
                    SeatsList = new List<SeatDomainModel>()
                },
                IsSuccessful = true
            };

            Task<CreateAuditoriumResultModel> responseTask = Task.FromResult(createAuditoriumResultModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(),
                It.IsAny<int>(),
                It.IsAny<int>())).Throws(dbUpdateException);

            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);

            //Act
            var result = auditoriumsController.PostAsync(createAuditoriumModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);

        }


        [TestMethod]
        public void PostAsync_Create_createAuditoriumResultModel_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Error occured while creating new auditorium, please try again.";
            int expectedStatusCode = 400;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel
            {
                auditName = "Auditorium55",
                cinemaId = 24,
                numberOfSeats = 5,
                seatRows = 6
            };

            CreateAuditoriumResultModel createAuditoriumResultModel = new CreateAuditoriumResultModel
            {
                Auditorium = new AuditoriumDomainModel
                {
                    Id = 123,
                    CinemaId = createAuditoriumModel.cinemaId,
                    Name = createAuditoriumModel.auditName,
                    SeatsList = new List<SeatDomainModel>()
                },
                IsSuccessful = false,
                ErrorMessage = Messages.AUDITORIUM_CREATION_ERROR
            };
            Task<CreateAuditoriumResultModel> responseTask = Task.FromResult(createAuditoriumResultModel);

            _auditoriumService = new Mock<IAuditoriumService>();
            _auditoriumService.Setup(x => x.CreateAuditorium(It.IsAny<AuditoriumDomainModel>(),
                It.IsAny<int>(),
                It.IsAny<int>())).Returns(responseTask);
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);


            //Act
            var result = auditoriumsController.PostAsync(createAuditoriumModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void PostAsync_With_InValid_ModelState_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateAuditoriumModel createAuditoriumModel = new CreateAuditoriumModel
            {
                auditName = "auditorium123",
                cinemaId = 44,
                numberOfSeats = 5,
                seatRows = 55
            };


            _auditoriumService = new Mock<IAuditoriumService>();
            AuditoriumsController auditoriumsController = new AuditoriumsController(_auditoriumService.Object);
            auditoriumsController.ModelState.AddModelError("key", "Invalid Model State");


            //Act
            var result = auditoriumsController.PostAsync(createAuditoriumModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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

        //Get auditorium by cinema ID remaining.


    }
}
