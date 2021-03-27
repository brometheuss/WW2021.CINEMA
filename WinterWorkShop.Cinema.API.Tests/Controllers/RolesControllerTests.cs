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
    public class RolesControllerTests
    {
        private Mock<IRoleService> _roleService;

        [TestMethod]
        public void Get_All_Async_Return_Roles_List()
        {
            //Arrange
            RoleDomainModel roleDomainModel = new RoleDomainModel
            {
                Id = 123,
                Name = "admin",
                Users = new List<UserDomainModel>()
            };
            int expectedStatusCode = 200;
            int expectedResultCount = 1;

            List<RoleDomainModel> roleDomainModels = new List<RoleDomainModel>();
            roleDomainModels.Add(roleDomainModel);

            IEnumerable<RoleDomainModel> rolesIEn = roleDomainModels;
            Task<IEnumerable<RoleDomainModel>> responseTask = Task.FromResult(rolesIEn);

            _roleService = new Mock<IRoleService>();
            _roleService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            RolesController rolesController = new RolesController(_roleService.Object);

            //Act
            var result = rolesController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<RoleDomainModel> roleDomainModelsResult = (List<RoleDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(roleDomainModelsResult);
            Assert.AreEqual(expectedResultCount, roleDomainModelsResult.Count);
            Assert.AreEqual(roleDomainModel.Id, roleDomainModelsResult[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void Get_All_Return_Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;
           
            IEnumerable<RoleDomainModel> rolesIEn = null;
            Task<IEnumerable<RoleDomainModel>> responseTask = Task.FromResult(rolesIEn);

            _roleService = new Mock<IRoleService>();
            _roleService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            RolesController rolesController = new RolesController(_roleService.Object);

            //Act
            var result = rolesController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<RoleDomainModel> roleDomainModelsResult = (List<RoleDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(roleDomainModelsResult);   
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetByIdAsync_Return_Role()
        {
            //Arrange
            int expectedStatusCode = 200;

            RoleDomainModel roleDomainModel = new RoleDomainModel
            {
                Id = 123,
                Name = "admin",
                Users = new List<UserDomainModel>()
            };

            Task<RoleDomainModel> responseTask = Task.FromResult(roleDomainModel);

            _roleService = new Mock<IRoleService>();
            _roleService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(responseTask);
            RolesController rolesController = new RolesController(_roleService.Object);

            //Act
            var result = rolesController.GetByIdAsync(roleDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            RoleDomainModel roleResult = (RoleDomainModel)resultList;

            //Assert
            Assert.IsNotNull(roleResult);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetByIdAsync_Return_NotFound()
        {
            //Arrange
            int expectedStatusCode = 404;
            string expectedMessage = Messages.ROLE_DOES_NOT_EXIST;

            RoleDomainModel roleDomainModel = null;

            Task<RoleDomainModel> responseTask = Task.FromResult(roleDomainModel);

            _roleService = new Mock<IRoleService>();
            _roleService.Setup(x => x.GetByIdAsync(It.IsAny<int>())).Returns(responseTask);
            RolesController rolesController = new RolesController(_roleService.Object);

            //Act
            var result = rolesController.GetByIdAsync(123).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((NotFoundObjectResult)result).Value;
            string roleResult = (string)resultList;

            //Assert
            Assert.IsNotNull(roleResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
            Assert.AreEqual(expectedMessage, roleResult);
        }
    }
}
