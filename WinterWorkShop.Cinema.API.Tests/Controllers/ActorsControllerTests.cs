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
    public class ActorsControllerTests
    {
        private Mock<IActorService> _actorService;

        [TestMethod]
        public void GetAsync_Return_All_Actors()
        {
            //Arrange
            ActorDomainModel actorDomainModel = new ActorDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "milan",
                LastName = "petrovic"
            };
            int expectedStatusCode = 200;
            int expectedResultCount = 1;

            List<ActorDomainModel> actorDomainModels = new List<ActorDomainModel>();
            actorDomainModels.Add(actorDomainModel);

            IEnumerable<ActorDomainModel> actorsIEn = actorDomainModels;
            Task<IEnumerable<ActorDomainModel>> responseTask = Task.FromResult(actorsIEn);

            _actorService = new Mock<IActorService>();
            _actorService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            ActorsController actorsController = new ActorsController(_actorService.Object);

            //Act
            var result = actorsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<ActorDomainModel> actorDomainModelsResult = (List<ActorDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(actorDomainModelsResult);
            Assert.AreEqual(expectedResultCount, actorDomainModelsResult.Count);
            Assert.AreEqual(actorDomainModel.Id, actorDomainModelsResult[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void Get_Async_Return_Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;
  
            IEnumerable<ActorDomainModel> actorsIEn = null;
            Task<IEnumerable<ActorDomainModel>> responseTask = Task.FromResult(actorsIEn);

            _actorService = new Mock<IActorService>();
            _actorService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            ActorsController actorsController = new ActorsController(_actorService.Object);

            //Act
            var result = actorsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<ActorDomainModel> actorDomainModelsResult = (List<ActorDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(actorDomainModelsResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetByIdAsync_Return_Actor_OkObjectResult()
        {
            //Arrange
            ActorDomainModel actorDomainModel = new ActorDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "milan",
                LastName = "petrovic"
            };
            int expectedStatusCode = 200;
 
            Task<ActorDomainModel> responseTask = Task.FromResult(actorDomainModel);

            _actorService = new Mock<IActorService>();
            _actorService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            ActorsController actorsController = new ActorsController(_actorService.Object);

            //Act
            var result = actorsController.GetById(actorDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            ActorDomainModel actorDomainModelsResult = (ActorDomainModel)resultList;

            //Assert
            Assert.IsNotNull(actorDomainModelsResult);
            Assert.AreEqual(actorDomainModel.FirstName, actorDomainModelsResult.FirstName);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetByIdAsync_Return_Not_Found()
        {
            //Arrange
            ActorDomainModel actorDomainModel = null;
            int expectedStatusCode = 404;
            string expectedMessage = Messages.ACTOR_NOT_FOUND;

            Task<ActorDomainModel> responseTask = Task.FromResult(actorDomainModel);

            _actorService = new Mock<IActorService>();
            _actorService.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            ActorsController actorsController = new ActorsController(_actorService.Object);

            //Act
            var result = actorsController.GetById(Guid.NewGuid()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((NotFoundObjectResult)result).Value;
            string actorDomainModelsResult = (string)resultList;

            //Assert
            Assert.IsNotNull(actorDomainModelsResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
            Assert.AreEqual(expectedMessage, actorDomainModelsResult);
        }
    }
}
