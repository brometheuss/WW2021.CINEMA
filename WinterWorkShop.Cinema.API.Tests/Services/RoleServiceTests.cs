using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class RoleServiceTests
    {
        private Mock<IRolesRepository> _mockRolesRepository;
        private Role _role;
        private RoleDomainModel _roleDomainModel;
        private RoleService _roleService;

        [TestInitialize]
        public void TestInitialize()
        {
            _role = new Role
            {
                Id = 1,
                Name = "normal user",
                Users = new List<User>()
            };

            _roleDomainModel = new RoleDomainModel
            {
                Id = 222,
                Name = "desi desi",
                Users = new List<UserDomainModel>()
            };

            _mockRolesRepository = new Mock<IRolesRepository>();
            _roleService = new RoleService(_mockRolesRepository.Object);
        }

        //GetAllRoles tests
        [TestMethod]
        public void RoleService_GetAllRoles_ReturnsListOfRoles()
        {
            //Arrange
            var expectedCount = 2;
            var userCount = 1;
            Role role2 = new Role
            {
                Id = 2,
                Name = "super user",
                Users = new List<User>()
            };
            User user1 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "pera",
                LastName = "Peric",
                Points = 230,
                RoleId = 2,
                IsAdmin = true,
                UserName = "Perica"
            };
            User user2 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Mika",
                LastName = "mikic",
                Points = 890,
                RoleId = 1,
                IsAdmin = false,
                UserName = "Mikela"
            };

            _role.Users.Add(user1);
            role2.Users.Add(user2);

            List<Role> roles = new List<Role>();
            roles.Add(_role);
            roles.Add(role2);

            _mockRolesRepository.Setup(x => x.GetAll()).ReturnsAsync(roles);

            //Act
            var resultAction = _roleService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(result[0].Id, _role.Id);
            Assert.AreEqual(result[1].Id, role2.Id);
            Assert.AreEqual(userCount, result[0].Users.Count);
            Assert.AreEqual(userCount, result[1].Users.Count);
            Assert.IsInstanceOfType(result[0], typeof(RoleDomainModel));
            Assert.IsInstanceOfType(result[0].Users[0], typeof(UserDomainModel));
        }

        [TestMethod]
        public void RoleService_GetAllRoles_ReturnsNull()
        {
            //Arrange
            List<Role> roles = new List<Role>();
            _mockRolesRepository.Setup(x => x.GetAll()).ReturnsAsync(roles);

            //Act
            var resultAction = _roleService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //GetRoleById tests
        [TestMethod]
        public void RoleService_GetByIdAsync_ReturnsRole()
        {
            //Arrange
            var expectedCount = 2;
            User user1 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Pera",
                LastName = "Peric",
                IsAdmin = true,
                Points = 1000,
                RoleId = 1,
                UserName = "perica"
            };
            User user2 = new User
            {
                Id = Guid.NewGuid(),
                FirstName = "Laza",
                LastName = "Lazic",
                IsAdmin = false,
                Points = 6700,
                RoleId = 1,
                UserName = "lazoni1233"
            };
            _role.Users.Add(user1);
            _role.Users.Add(user2);
            _mockRolesRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_role);

            //Act
            var resultAction = _roleService.GetByIdAsync(_role.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_role.Id, resultAction.Id);
            Assert.AreEqual(expectedCount, resultAction.Users.Count);
            Assert.AreEqual(user1.UserName, resultAction.Users[0].UserName);
            Assert.AreEqual(user2.UserName, resultAction.Users[1].UserName);
            Assert.IsInstanceOfType(resultAction, typeof(RoleDomainModel));
            Assert.IsInstanceOfType(resultAction.Users[0], typeof(UserDomainModel));
        }

        [TestMethod]
        public void RoleService_GetByIdAsync_ReturnsNull()
        {
            //Arrange
            _mockRolesRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Role);

            //Act
            var resultAction = _roleService.GetByIdAsync(_role.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }
    }
}
