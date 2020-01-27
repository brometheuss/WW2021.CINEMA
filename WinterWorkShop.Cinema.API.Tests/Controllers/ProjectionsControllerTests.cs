using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class ProjectionsControllerTests
    {
        private Mock<IProjectionService> _projectionService;
        

        [TestMethod]
        public void GetAsync_Return_All_Projections() 
        {
            //Arrange
            List<ProjectionDomainModel> projectionsDomainModelsList = new List<ProjectionDomainModel>();
            ProjectionDomainModel projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };
            projectionsDomainModelsList.Add(projectionDomainModel);
            IEnumerable<ProjectionDomainModel> projectionDomainModels = projectionsDomainModelsList;            
            var responseTask = Task.FromResult(projectionDomainModels);
            var expectedResultCount = 1;
            var expectedStatusCode = 200;

            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult) result).Value;
            List<ProjectionDomainModel> projectionDomainModelResultList = (List<ProjectionDomainModel>)resultList;

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
            var responseTask = Task.FromResult(projectionDomainModels);
            var expectedResult = true;
            var expectedStatusCode = 200;

            _projectionService = new Mock<IProjectionService>();
            _projectionService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            ProjectionsController projectionsController = new ProjectionsController(_projectionService.Object);

            //Act
            var result = projectionsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(result);
            //Assert.AreEqual(expectedResult, result.IsCompleted);
            //Assert.IsInstanceOfType(result.GetResult().Result, typeof(Microsoft.AspNetCore.Mvc.OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((Microsoft.AspNetCore.Mvc.OkObjectResult)result.Result).StatusCode);
        }
    }
}
