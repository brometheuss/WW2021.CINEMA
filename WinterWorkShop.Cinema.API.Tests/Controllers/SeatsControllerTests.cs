using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using WinterWorkShop.Cinema.Domain.Common;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class SeatsControllerTests
    {
        private Mock<ISeatService> _seatService;
        private Mock<IReservationService> _reservationService;

        [TestMethod]
        public void GetAsync_Return_All_Seats()
        {
            //Arrange
            SeatDomainModel seatDomainModel = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 23,
                Number = 2,
                Row = 3
            };
            int expectedStatusCode = 200;
            int expectedResultCount = 1;

            List<SeatDomainModel> seatDomainModels = new List<SeatDomainModel>();
            seatDomainModels.Add(seatDomainModel);

            IEnumerable<SeatDomainModel> seatsIEn = seatDomainModels;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seatsIEn);

            _seatService = new Mock<ISeatService>();
            _reservationService = new Mock<IReservationService>();
            _seatService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object, _reservationService.Object);

            //Act
            var result = seatsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<SeatDomainModel> seatDomainModelsResult = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(seatDomainModelsResult);
            Assert.AreEqual(expectedResultCount, seatDomainModelsResult.Count);
            Assert.AreEqual(seatDomainModel.Id, seatDomainModelsResult[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }
        [TestMethod]
        public void GetAsync_Return_Empty_List()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedResultCount = 0;

            List<SeatDomainModel> seatDomainModels = null;

            IEnumerable<SeatDomainModel> seatsIEn = seatDomainModels;
            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(seatsIEn);

            _seatService = new Mock<ISeatService>();
            _reservationService = new Mock<IReservationService>();
            _seatService.Setup(x => x.GetAllAsync()).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object, _reservationService.Object);

            //Act
            var result = seatsController.GetAsync().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            List<SeatDomainModel> seatDomainModelsResult = (List<SeatDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(seatDomainModelsResult);
            Assert.AreEqual(expectedResultCount, seatDomainModelsResult.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void GetNumber_Of_Seats_Return_All()
        {
            //Arrange
            int expectedRow = 5;
            int expectedNumber = 6;
            int expectedStatusCode = 200;

            SeatAuditoriumDomainModel seatAuditoriumDomainModel = new SeatAuditoriumDomainModel
            {
                MaxNumber = 6,
                MaxRow = 5,
                Seats = new List<SeatDomainModel>()
            };

            SeatAuditoriumDomainModel listOfSeatsIEn = seatAuditoriumDomainModel;

            Task<SeatAuditoriumDomainModel> responseTask = Task.FromResult(listOfSeatsIEn);

            _seatService = new Mock<ISeatService>();
            _reservationService = new Mock<IReservationService>();
            _seatService.Setup(x => x.GetAllSeatsForAuditorium(It.IsAny<int>())).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object, _reservationService.Object);

            //Act
            var result = seatsController.GetNumberOfSeats(1).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((OkObjectResult)result).Value;
            SeatAuditoriumDomainModel seatAuditoriumResult = (SeatAuditoriumDomainModel)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(expectedRow, seatAuditoriumResult.MaxRow);
            Assert.AreEqual(expectedNumber, seatAuditoriumResult.MaxNumber);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }
        [TestMethod]
        public void GetNumber_Of_Seats_Return_NotFound()
        {
            //Arrange
            int expectedStatusCode = 404;
            string expectedMessage = Messages.SEAT_AUDITORIUM_NOT_FOUND;


            SeatAuditoriumDomainModel seatAuditoriumDomainModel = null;

            Task<SeatAuditoriumDomainModel> responseTask = Task.FromResult(seatAuditoriumDomainModel);

            _seatService = new Mock<ISeatService>();
            _reservationService = new Mock<IReservationService>();
            _seatService.Setup(x => x.GetAllSeatsForAuditorium(It.IsAny<int>())).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object, _reservationService.Object);

            //Act
            var result = seatsController.GetNumberOfSeats(1).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((NotFoundObjectResult)result).Value;
            string seatAuditoriumResult = (string)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(expectedMessage, seatAuditoriumResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAll_Seats_By_Auditorium_Id()
        {
            //Arrange
            int expectedStatusCode = 200;
            int expectedRow = 3;
            int expectedNumber = 2;

            SeatDomainModel seatDomainModel = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 23,
                Number = 2,
                Row = 3
            };

            List<SeatDomainModel> seatDomainModels = new List<SeatDomainModel>();
            seatDomainModels.Add(seatDomainModel);
            

            IEnumerable<SeatDomainModel> listOfSeatsIEn = seatDomainModels;

            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(listOfSeatsIEn);

            _seatService = new Mock<ISeatService>();
            _reservationService = new Mock<IReservationService>();
            _seatService.Setup(x => x.GetAllSeatsByAuditoriumId(It.IsAny<int>())).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object, _reservationService.Object);

            //Act
            var result = seatsController.GetAllSeatsByAuditoriumId(1).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((OkObjectResult)result).Value;
            List<SeatDomainModel> seatsResult = (List<SeatDomainModel>)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.AreEqual(expectedRow, seatsResult[0].Row);
            Assert.AreEqual(expectedNumber, seatsResult[0].Number);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void GetAll_Seats_By_Auditorium_Id_Return_Not_Found()
        {
            //Arrange
            int expectedStatusCode = 404;
        
            IEnumerable<SeatDomainModel> listOfSeatsIEn = null;

            Task<IEnumerable<SeatDomainModel>> responseTask = Task.FromResult(listOfSeatsIEn);

            _seatService = new Mock<ISeatService>();
            _reservationService = new Mock<IReservationService>();
            _seatService.Setup(x => x.GetAllSeatsByAuditoriumId(It.IsAny<int>())).Returns(responseTask);
            SeatsController seatsController = new SeatsController(_seatService.Object, _reservationService.Object);

            //Act
            var result = seatsController.GetAllSeatsByAuditoriumId(1).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((NotFoundObjectResult)result).Value;
            string seatsResult = (string)objectResult;

            //Assert
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(result, typeof(NotFoundObjectResult));
            Assert.AreEqual(expectedStatusCode, ((NotFoundObjectResult)result).StatusCode);
        }
    }
}
