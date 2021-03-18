using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class AuditoriumServiceTests
    {
        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private Mock<ICinemasRepository> _mockCinemaRepository;
        private Auditorium _auditorium;
        private AuditoriumDomainModel _auditoriumDomainModel;
        private Data.Cinema _cinema;
        private CinemaDomainModel _cinemaDomainModel;
        private AuditoriumService _auditoriumService;
        

        [TestInitialize]
        public void TestInitialize()
        {
            _auditorium = new Auditorium
            {
                Id = 1,
                CinemaId = 1,
                AuditName = "Novi auditorium",
            };

            _auditoriumDomainModel = new AuditoriumDomainModel
            {
                Id = _auditorium.Id,
                CinemaId = _auditorium.CinemaId,
                Name = _auditorium.AuditName
            };

            _cinema = new Data.Cinema
            {
                Id = 1,
                CityId = 1,
                Name = "test bioskop 1"
            };

            _cinemaDomainModel = new CinemaDomainModel
            {
                Id = _cinema.Id,
                CityId = _cinema.CityId,
                Name = _cinema.Name
            };

            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockCinemaRepository = new Mock<ICinemasRepository>();
            _auditoriumService = new AuditoriumService(_mockAuditoriumRepository.Object, _mockCinemaRepository.Object);
        }

        //GetAllAuditoriums tests
        [TestMethod]
        public void AuditoriumService_GetAllAuditoriums_ReturnsListOfAuditoriums()
        {
            //Arrange
            var expectedCount = 2;

            Auditorium auditorium2 = new Auditorium
            {
                Id = 2,
                CinemaId = 2,
                AuditName = "Stari auditorium"
            };

            List<Auditorium> auditoriums = new List<Auditorium>();
            auditoriums.Add(_auditorium);
            auditoriums.Add(auditorium2);

            List<AuditoriumDomainModel> auditoriumDomainModels = new List<AuditoriumDomainModel>();
            auditoriumDomainModels.Add(_auditoriumDomainModel);

            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditoriums);

            //Act
            var resultAction = _auditoriumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(result[0].Id, _auditoriumDomainModel.Id);
            Assert.IsInstanceOfType(result[0], typeof(AuditoriumDomainModel));
        }

        [TestMethod]
        public void AuditoriumService_GetAllAuditoriums_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<Auditorium> auditoriums = new List<Auditorium>();

            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditoriums);

            //Act
            var resultAction = _auditoriumService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }

        //CreateAuditorium tests
        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsCreatedAuditoriumWithSeats()
        {
            //Arrange
            var expectedSeatCount = 3;
            int numOfRows = 2;
            int numOfSeats = 4;

            Seat s1 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 1
            };

            Seat s2 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 1
            };

            Seat s3 = new Seat
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 2
            };

            SeatDomainModel s4 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 2
            };
            SeatDomainModel s5 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 2
            };
            SeatDomainModel s6 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 1,
                Row = 2
            };

            List<Auditorium> audits = new List<Auditorium>();

            List <Seat> seats = new List<Seat>();
            List <SeatDomainModel> seatsList = new List<SeatDomainModel>();
            seats.Add(s1);
            seats.Add(s2);
            seats.Add(s3);

            seatsList.Add(s4);
            seatsList.Add(s5);
            seatsList.Add(s6);

            _auditorium.Seats = seats;
            _auditoriumDomainModel.SeatsList = seatsList;
            AuditoriumDomainModel ad2 = new AuditoriumDomainModel
            {
                Id = 22,
                CinemaId = 2,
                Name = "bla bla audit",
                SeatsList = seatsList
            };

            CreateAuditoriumResultModel cr = new CreateAuditoriumResultModel
            {
                IsSuccessful = true,
                ErrorMessage = null,
                Auditorium = _auditoriumDomainModel
            };

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.Insert(It.IsAny<Auditorium>())).Returns(_auditorium);
            _mockAuditoriumRepository.Setup(x => x.GetByAuditName(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(audits);
            _mockAuditoriumRepository.Setup(x => x.Insert(It.IsAny<Auditorium>())).Returns(_auditorium);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(ad2, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Arrange
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(resultAction.Auditorium.Id, cr.Auditorium.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
            Assert.AreEqual(expectedSeatCount, resultAction.Auditorium.SeatsList.Count);
            Assert.IsTrue(resultAction.IsSuccessful);
            Assert.IsNull(resultAction.ErrorMessage);
        }

        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsErrorMessage_InvalidCinema()
        {
            //Arrange
            var numOfRows = 2;
            var numOfSeats = 3;

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Data.Cinema);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(_auditoriumDomainModel, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction.Auditorium);
            Assert.AreEqual(Messages.AUDITORIUM_INVALID_CINEMAID, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
        }

        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsErrorMessage_AuditoriumSameName()
        {
            //Arrange
            var numOfRows = 2;
            var numOfSeats = 3;

            List<Auditorium> audits = new List<Auditorium>();
            audits.Add(_auditorium);

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetByAuditName(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(audits);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(_auditoriumDomainModel, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction.Auditorium);
            Assert.AreEqual(Messages.AUDITORIUM_SAME_NAME, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
        }

        [TestMethod]
        public void AuditoriumService_CreateAuditorium_ReturnsErrorMessage_AuditoriumCreationError()
        {
            //Arrange
            var numOfRows = 2;
            var numOfSeats = 3;
            List<Auditorium> audits = new List<Auditorium>();

            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetByAuditName(It.IsAny<string>(), It.IsAny<int>())).ReturnsAsync(audits);
            _mockAuditoriumRepository.Setup(x => x.Insert(It.IsAny<Auditorium>())).Returns(null as Auditorium);

            //Act
            var resultAction = _auditoriumService.CreateAuditorium(_auditoriumDomainModel, numOfRows, numOfSeats).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction.Auditorium);
            Assert.AreEqual(Messages.AUDITORIUM_CREATION_ERROR, resultAction.ErrorMessage);
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsInstanceOfType(resultAction, typeof(CreateAuditoriumResultModel));
        }

        //GetAuditoriumByCinemaId tests
        [TestMethod]
        public void AuditoriumService_GetAuditoriumsByCinemaId_ReturnsListOfAuditoriums()
        {
            //Arrange
            var expectedCount = 2;
            Auditorium audit2 = new Auditorium
            {
                Id = 111,
                CinemaId = 1,
                AuditName = "Dodatni auditorium"
            };

            List<Auditorium> audits = new List<Auditorium>();
            audits.Add(_auditorium);
            audits.Add(audit2);

            _mockAuditoriumRepository.Setup(x => x.GetAuditoriumsByCinemaId(It.IsAny<int>())).Returns(audits);

            //Act
            var resultAction = _auditoriumService.GetAuditoriumsByCinemaId(_cinema.Id);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(result[0].Id, _auditorium.Id);
            Assert.AreEqual(result[1].Name, audit2.AuditName);
            Assert.IsInstanceOfType(result[0], typeof(AuditoriumDomainModel));
        }

        [TestMethod]
        public void AuditoriumService_GetAuditoriumsByCinemaId_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<Auditorium> audits = new List<Auditorium>();

            _mockAuditoriumRepository.Setup(x => x.GetAuditoriumsByCinemaId(It.IsAny<int>())).Returns(audits);

            //Act
            var resultAction = _auditoriumService.GetAuditoriumsByCinemaId(_cinema.Id);
            var result = resultAction.ToList();

            //Assert
            Assert.AreEqual(expectedCount, result.Count);
        }
    }
}
