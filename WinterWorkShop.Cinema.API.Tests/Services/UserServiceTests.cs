using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IUsersRepository> _mockUserRepository;
        private Mock<IRolesRepository> _mockRoleRepository;
        private UserService _userService;
        private User _user;
        private UserDomainModel _userDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _user = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Uros",
                LastName = "Markov",
                IsAdmin = false,
                Points = 2900,
                RoleId = 2,
                UserName = "Brometheus"
            };

            _userDomainModel = new UserDomainModel
            {
                Id = _user.Id,
                FirstName = _user.FirstName,
                LastName = _user.LastName,
                IsAdmin = _user.IsAdmin,
                Points = _user.Points,
                RoleId = _user.RoleId,
                UserName = _user.UserName
            };

            _mockUserRepository = new Mock<IUsersRepository>();
            _mockRoleRepository = new Mock<IRolesRepository>();

            _userService = new UserService(_mockUserRepository.Object);
        }

        //GetAllUsers tests
        [TestMethod]
        public void UserService_GetAllUsers_ReturnsListOfUsers()
        {
            //Arrange
            var expectedCount = 2;

            User user2 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Pera",
                LastName = "Peric",
                IsAdmin = true,
                Points = 500,
                RoleId = 1,
                UserName = "perica"
            };

            List<User> userList = new List<User>();
            userList.Add(_user);
            userList.Add(user2);
            _mockUserRepository.Setup(x => x.GetAll()).ReturnsAsync(userList);

            //Act
            var resultAction = _userService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(_user.Id, result[0].Id);
            Assert.AreEqual(user2.Id, result[1].Id);
            Assert.IsInstanceOfType(result[0], typeof(UserDomainModel));
        }

        [TestMethod]
        public void UserService_GetAllUsers_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;

            List<User> userList = new List<User>();
            _mockUserRepository.Setup(x => x.GetAll()).ReturnsAsync(userList);

            //Act
            var resultAction = _userService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }

        //GetUserById tests
        [TestMethod]
        public void UserSevice_GetUserById_ReturnsUser()
        {
            //Arrange
            _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_user);

            //Act
            var resultAction = _userService.GetUserByIdAsync(_user.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Id, _user.Id);
            Assert.IsFalse(resultAction.IsAdmin);
        }

        [TestMethod]
        public void UserService_GetUserByid_ReturnsNull()
        {
            //Arrange
            _mockUserRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as User);

            //Act
            var resultAction = _userService.GetUserByIdAsync(_user.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //GetUserByUsername  tests
        [TestMethod]
        public void UserService_GetUserByUsername_ReturnsUser()
        {
            //Arrange
            _mockUserRepository.Setup(x => x.GetByUserName(It.IsAny<string>())).ReturnsAsync(_user);

            //Act
            var resultAction = _userService.GetUserByUserName(_user.UserName).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_user.UserName, resultAction.UserName);
            Assert.IsInstanceOfType(resultAction, typeof(UserDomainModel));
        }

        [TestMethod]
        public void UserService_GetUserByUsername_ReturnNull()
        {
            //Arrange
            _mockUserRepository.Setup(x => x.GetByUserName(It.IsAny<string>())).ReturnsAsync(null as User);

            //Act
            var resultAction = _userService.GetUserByUserName(_user.UserName).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }
    }
}
