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
using WinterWorkShop.Cinema.Domain.Queries;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class ProjectionsControllerTests
    {
        private Mock<IProjectionService> _projectionService;
        private ProjectionQuery query;


        [TestMethod]
        public void GetAsync_Return_All_Projections()
        {
            //Arrange
            List<ProjectionDomainModel> projectionsDomainModelsList = new List<ProjectionDomainModel>();
            ProjectionDomainModel projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };

            query = new ProjectionQuery();

            projectionsDomainModelsList.Add(projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projectionDomainModels = projectionsDomainModelsList;
            Task<IEnumerable<ProjectionDomainModel>> responseTask = Task.FromResult(projectionDomainModels);
            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.GetAllAsync(query)).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.GetAsync(query).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var projectionDomainModelResultList = (List<ProjectionDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(projectionDomainModelResultList);
            Assert.AreEqual(expectedResultCount, projectionDomainModelResultList.Count);
            Assert.AreEqual(projectionDomainModel.Id, projectionDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_NewList()
        {
            //Arrange
            IEnumerable<ProjectionDomainModel> projectionDomainModels = null;
            Task<IEnumerable<ProjectionDomainModel>> responseTask = Task.FromResult(projectionDomainModels);
            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.GetAllAsync(query)).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.GetAsync(query).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var projectionDomainModelResultList = (List<ProjectionDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(projectionDomainModelResultList);
            Assert.AreEqual(expectedResultCount, projectionDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - false
        // try  await _projectionService.CreateProjection(domainModel) - return valid mock
        // if (!createProjectionResultModel.IsSuccessful) - false
        // return Created
        [TestMethod]
        public void PostAsync_Create_createProjectionResultModel_IsSuccessful_True_Projection() 
        {
            //Arrange
            int expectedStatusCode = 201;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };
            CreateProjectionResultModel createProjectionResultModel = new CreateProjectionResultModel
            {
                Projection = new ProjectionDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumName = "ImeSale",
                    AuditoriumId = createProjectionModel.AuditoriumId,
                    MovieId = createProjectionModel.MovieId,
                    MovieTitle = "ImeFilma",
                    ProjectionTime = createProjectionModel.ProjectionTime
                },
                IsSuccessful = true
            };
            Task<CreateProjectionResultModel> responseTask = Task.FromResult(createProjectionResultModel);


            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.CreateProjection(It.IsAny<ProjectionDomainModel>())).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var createdResult = ((CreatedResult)result).Value;
            var projectionDomainModel = (ProjectionDomainModel)createdResult;

            //Assert
            Assert.IsNotNull(projectionDomainModel);
            Assert.AreEqual(createProjectionModel.MovieId, projectionDomainModel.MovieId);
            Assert.IsInstanceOfType(result, typeof(CreatedResult));
            Assert.AreEqual(expectedStatusCode, ((CreatedResult)result).StatusCode);
        }

        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - false
        // try  await _projectionService.CreateProjection(domainModel) - throw DbUpdateException
        // return BadRequest
        [TestMethod]
        public void PostAsync_Create_Throw_DbException_Projection()
        {
            //Arrange
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };
            CreateProjectionResultModel createProjectionResultModel = new CreateProjectionResultModel
            {
                Projection = new ProjectionDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumName = "ImeSale",
                    AuditoriumId = createProjectionModel.AuditoriumId,
                    MovieId = createProjectionModel.MovieId,
                    MovieTitle = "ImeFilma",
                    ProjectionTime = createProjectionModel.ProjectionTime
                },
                IsSuccessful = true
            };
            Task<CreateProjectionResultModel> responseTask = Task.FromResult(createProjectionResultModel);
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);

            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.CreateProjection(It.IsAny<ProjectionDomainModel>())).Throws(dbUpdateException);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }


        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - false
        // try  await _projectionService.CreateProjection(domainModel) - return valid mock
        // if (!createProjectionResultModel.IsSuccessful) - true
        // return BadRequest
        [TestMethod]
        public void PostAsync_Create_createProjectionResultModel_IsSuccessful_False_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Error occured while creating new projection, please try again.";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 1
            };
            CreateProjectionResultModel createProjectionResultModel = new CreateProjectionResultModel
            {
                Projection = new ProjectionDomainModel
                {
                    Id = Guid.NewGuid(),
                    AuditoriumName = "ImeSale",
                    AuditoriumId = createProjectionModel.AuditoriumId,
                    MovieId = createProjectionModel.MovieId,
                    MovieTitle = "ImeFilma",
                    ProjectionTime = createProjectionModel.ProjectionTime
                },
                IsSuccessful = false,
                ErrorMessage = Messages.PROJECTION_CREATION_ERROR,
            };
            Task<CreateProjectionResultModel> responseTask = Task.FromResult(createProjectionResultModel);


            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.CreateProjection(It.IsAny<ProjectionDomainModel>())).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var badObjectResult = ((BadRequestObjectResult)result).Value;
            var errorResult = (ErrorResponseModel)badObjectResult;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, errorResult.ErrorMessage);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        // if (!ModelState.IsValid) - true
        // return BadRequest
        [TestMethod]
        public void PostAsync_With_UnValid_ModelState_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Invalid Model State";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(1),
                AuditoriumId = 0
            };

            _projectionService = new Mock<IProjectionService>();
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);
            projectionsController.ModelState.AddModelError("key","Invalid Model State");

            //Act
            var result = projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
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

        // if (!ModelState.IsValid) - false
        // if (projectionModel.ProjectionTime < DateTime.Now) - true
        // return BadRequest
        [TestMethod]
        public void PostAsync_With_UnValid_ProjectionDate_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = "Projection time cannot be in past.";
            int expectedStatusCode = 400;

            CreateProjectionModel createProjectionModel = new CreateProjectionModel()
            {
                MovieId = Guid.NewGuid(),
                ProjectionTime = DateTime.Now.AddDays(-1),
                AuditoriumId = 0
            };

            _projectionService = new Mock<IProjectionService>();
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.PostAsync(createProjectionModel).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultResponse = (BadRequestObjectResult)result;
            var createdResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = ((SerializableError)createdResult).GetValueOrDefault(nameof(createProjectionModel.ProjectionTime));
            var message = (string[])errorResponse;

            //Assert
            Assert.IsNotNull(resultResponse);
            Assert.AreEqual(expectedMessage, message[0]);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            Assert.AreEqual(expectedStatusCode, resultResponse.StatusCode);
        }

        [TestMethod]
        public void Get_By_Projection_Id_Return_Projection()
        {
            //Assert
            int expectedStatusCode = 200;

            ProjectionDomainModel projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumName = "Auditorium123",
                AuditoriumId = 2,
                MovieId = Guid.NewGuid(),
                MovieTitle = "Film",
                ProjectionTime = new DateTime()
            };

            Task<ProjectionDomainModel> responseTask = Task.FromResult(projectionDomainModel);

            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.GetProjectionByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.GetById(projectionDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultValue = ((OkObjectResult)result).Value;
            var projectionDomainModelResult = (ProjectionDomainModel)resultValue;

            //Assert
            Assert.IsNotNull(projectionDomainModelResult);
            Assert.AreEqual(projectionDomainModel.Id, projectionDomainModelResult.Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }

        [TestMethod]
        public void Get_By_Projection_Id_Return_Not_Found()
        {
            //Arrange
            int expectedStatusCode = 404;
            string expectedMessage = Messages.PROJECTION_DOES_NOT_EXIST;
            ProjectionDomainModel projectionDomainModel = null;


            Task<ProjectionDomainModel> responseTask = Task.FromResult(projectionDomainModel);

            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.GetProjectionByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.GetById(Guid.NewGuid()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultValue = ((NotFoundObjectResult)result).Value;
            var messageReturned = (string)resultValue;

            //Assert
            Assert.IsNotNull(messageReturned);
            Assert.AreEqual(expectedMessage, messageReturned);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
        }
    }
}
