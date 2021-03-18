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
    public class UsersControllerTests
    {
        private Mock<IUserService> _userService;

        [TestMethod]
        public void Get_Async_Return_All_Users()
        {
            //Arrange
            List<UserDomainModel> usersDomainModelsList = new List<UserDomainModel>();


            UserDomainModel userDomainModel = new UserDomainModel
            {
                Id = Guid.NewGuid(),
                IsAdmin = false,
                FirstName = "milan",
                LastName = "milenovic",
                Points = 5,
                RoleId = 2,
                UserName = "milanmilenovic"
            };

            usersDomainModelsList.Add(userDomainModel);
            IEnumerable<UserDomainModel> userDomainModels = usersDomainModelsList;

            Task<IEnumerable<UserDomainModel>> responseTask = Task
               .FromResult(userDomainModels);

            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            UsersController usersController = new UsersController(_userService.Object);

            //Act
            var result = usersController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var userDomainModelResultList = (List<UserDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(userDomainModelResultList);
            Assert.AreEqual(expectedResultCount, userDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }

        [TestMethod]
        public void Get_Async_Return_New_List()
        {
            //Arrange
            IEnumerable<UserDomainModel> userDomainModels = null;
            Task<IEnumerable<UserDomainModel>> responseTask = Task.FromResult(userDomainModels);

            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            UsersController usersController = new UsersController(_userService.Object);

            //Act
            var result = usersController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var userDomainModelResultList = (List<UserDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(userDomainModelResultList);
            Assert.AreEqual(expectedResultCount, userDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);


        }

        [TestMethod]
        public void Get_Async_Return_User_By_Id()
        {
            //Arrange
            UserDomainModel userDomainModel = new UserDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "milos",
                LastName = "milosevic",
                IsAdmin = true,
                Points = 50,
                RoleId = 20,
                UserName = "milosevicmilos"
            };

            Task<UserDomainModel> responseTask = Task.FromResult(userDomainModel);

            int expectedStatusCode = 200;

            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            UsersController usersController = new UsersController(_userService.Object);

            //Act
            var result = usersController.GetbyIdAsync(userDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultValue = ((OkObjectResult)result).Value;
            var userDomainModelResult = (UserDomainModel)resultValue;

            //Assert
            Assert.IsNotNull(userDomainModelResult);
            Assert.AreEqual(userDomainModel.Id, userDomainModelResult.Id);
            Assert.AreEqual(userDomainModel.RoleId, userDomainModelResult.RoleId);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);


        }

        [TestMethod]
        public void Get_Async_Return_User_NotFount_By_Id()
        {
            //Arrange
            UserDomainModel userDomainModel = null;
            string expectedMessage = Messages.USER_NOT_FOUND;

            Task<UserDomainModel> responseTask = Task.FromResult(userDomainModel);

            int expectedStatusCode = 404;

            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetUserByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            UsersController usersController = new UsersController(_userService.Object);


            //Act
            var result = usersController.GetbyIdAsync(Guid.NewGuid()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultValue = ((NotFoundObjectResult)result).Value;
            var messageReturned = (string)resultValue;

            //Assert
            Assert.IsNotNull(messageReturned);
            Assert.AreEqual(expectedMessage, messageReturned);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void Get_User_By_UserName_Return_Successful()
        {
            UserDomainModel userDomainModel = new UserDomainModel
            {
                Id = Guid.NewGuid(),
                FirstName = "milos",
                LastName = "milosevic",
                IsAdmin = true,
                Points = 50,
                RoleId = 20,
                UserName = "milosevicmilos"
            };

            Task<UserDomainModel> responseTask = Task.FromResult(userDomainModel);
            int expectedStatusCode = 200;

            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetUserByUserName(It.IsAny<string>())).Returns(responseTask);
            UsersController usersController = new UsersController(_userService.Object);

            //Act
            var result = usersController.GetbyUserNameAsync(userDomainModel.UserName).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultObject = ((OkObjectResult)result).Value;
            var userDomainModelResult = (UserDomainModel)resultObject;

            //Assert
            Assert.IsNotNull(userDomainModelResult);
            Assert.AreEqual(userDomainModel.Id, userDomainModelResult.Id);
            Assert.AreEqual(userDomainModel.UserName, userDomainModelResult.UserName);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }

        [TestMethod]
        public void Get_User_By_UserName_Return_Not_Found()
        {
            //Arrange
            UserDomainModel userDomainModel = null;
            string expectedMessage = Messages.USER_NOT_FOUND;

            Task<UserDomainModel> responseTask = Task.FromResult(userDomainModel);

            int expectedStatusCode = 404;

            _userService = new Mock<IUserService>();
            _userService.Setup(x => x.GetUserByUserName(It.IsAny<string>())).Returns(responseTask);
            UsersController usersController = new UsersController(_userService.Object);


            //Act
            var result = usersController.GetbyUserNameAsync("asd").ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultObject = ((NotFoundObjectResult)result).Value;
            var messageReturned = (string)resultObject;

            //Assert
            Assert.IsNotNull(messageReturned);
            Assert.AreEqual(expectedMessage, messageReturned);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
        }

    }
}
