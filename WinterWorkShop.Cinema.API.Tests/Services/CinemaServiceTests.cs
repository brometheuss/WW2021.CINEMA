using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Data;
using Moq;
using WinterWorkShop.Cinema.Repositories;
using WinterWorkShop.Cinema.Domain.Services;
using System.Linq;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class CinemaServiceTests
    {
        private Mock<ICinemasRepository> _mockCinemaRepository;
        private Mock<IAuditoriumsRepository> _mockAuditoriumRepository;
        private Mock<ISeatsRepository> _mockSeatRepository;
        private Mock<ICitiesRepository> _mockCityRepository;
        private CinemaService cinemaService;
        private Data.Cinema _cinema;
        private City _city;
        private CinemaDomainModel _cinemaDomainModel;
        private CityDomainModel _cityDomainModel;


        [TestInitialize]
        public void TestInitialize()
        {
            _cinema = new Data.Cinema
            {
                Id = 11,
                CityId = 1,
                Name = "Smekerski bioskop"
            };

            _cinemaDomainModel = new CinemaDomainModel
            {
                Id = _cinema.Id,
                CityId = _cinema.CityId,
                Name = _cinema.Name
            };

            _mockCinemaRepository = new Mock<ICinemasRepository>();
            _mockAuditoriumRepository = new Mock<IAuditoriumsRepository>();
            _mockSeatRepository = new Mock<ISeatsRepository>();
            _mockCityRepository = new Mock<ICitiesRepository>();

            cinemaService = new CinemaService(_mockCinemaRepository.Object, _mockAuditoriumRepository.Object, _mockSeatRepository.Object, _mockCityRepository.Object);
        }

        //GetAllCinemas tests
        [TestMethod]
        public void CinemaService_GetAllCinemasAsync_ReturnsListOfCinemas()
        {
            //Arrange
            var expectedCount = 2;
            Data.Cinema cinema2 = new Data.Cinema
            {
                Id = 22,
                CityId = 1,
                Name = "Los bioskop"
            };
            CinemaDomainModel cinemaDomainModel2 = new CinemaDomainModel
            {
                Id = cinema2.Id,
                CityId = cinema2.CityId,
                Name = cinema2.Name
            };
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Add(_cinema);
            cinemas.Add(cinema2);
            _mockCinemaRepository.Setup(x => x.GetAll()).ReturnsAsync(cinemas);

            //Act
            var resultAction = cinemaService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.AreEqual(_cinema.Name, result[0].Name);
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<CinemaDomainModel>));
        }

        [TestMethod]
        public void CinemaService_GetAllCinemasAsync_ReturnsEmptyList()
        {
            //Arrange
            var expectedCount = 0;
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
            cinemas.Clear();
            _mockCinemaRepository.Setup(x => x.GetAll()).ReturnsAsync(cinemas);

            //Act
            var resultAction = cinemaService.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(resultAction, typeof(List<CinemaDomainModel>));
        }
        
        //DeleteCinema tests
        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsDeletedCinema()
        {
            //Arrange
            List<Seat> seats1 = new List<Seat>();
            List<Seat> seats2 = new List<Seat>();
            var seat1 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 90,
                Row = 10,
                AuditoriumId = 2                
            };
            var seat2 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 91,
                Row = 10,
                AuditoriumId = 2
            };
            var seat3 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 92,
                Row = 10,
                AuditoriumId = 3
            };
            seats1.Add(seat1);
            seats1.Add(seat2);
            seats2.Add(seat3);
          
            Auditorium audit1 = new Auditorium
            {
                Id = 2,
                AuditName = "Auditorium 1",
                CinemaId = 11,
                Seats = seats1
            };

            Auditorium audit2 = new Auditorium
            {
                Id = 3,
                AuditName = "Auditorium 2",
                CinemaId = 11,
                Seats = seats2
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);
            auditsList.Add(audit2);

            _mockCinemaRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_cinema);
            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditsList);

            //Act
            var resultAction = cinemaService.DeleteCinemaAsync(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            
            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_cinema.Id, resultAction.Id);
            Assert.IsInstanceOfType(resultAction, typeof(CinemaDomainModel));
        }

        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsNullCinema()
        {
            //Arrange
            Auditorium audit1 = new Auditorium
            {
                Id = 2,
                AuditName = "Auditorium 1",
                CinemaId = 11,
            };

            Auditorium audit2 = new Auditorium
            {
                Id = 3,
                AuditName = "Auditorium 2",
                CinemaId = 11,
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);
            auditsList.Add(audit2);


            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as Data.Cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditsList);
            _mockCinemaRepository.Setup(x => x.Delete(It.IsAny<int>())).Returns(_cinema);

            //Act
            var resultAction = cinemaService.DeleteCinemaAsync(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void CinemaService_DeleteCinemaAsync_ReturnsEmptyListAuditoriums()
        {
            //Arrange
            List<Seat> seats1 = new List<Seat>();
            List<Seat> seats2 = new List<Seat>();
            var seat1 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 90,
                Row = 10,
                AuditoriumId = 2
            };
            var seat2 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 91,
                Row = 10,
                AuditoriumId = 2
            };
            var seat3 = new Seat
            {
                Id = Guid.NewGuid(),
                Number = 92,
                Row = 10,
                AuditoriumId = 2
            };
            seats1.Add(seat1);
            seats1.Add(seat2);
            seats1.Add(seat3);

            Auditorium audit1 = new Auditorium
            {
                Id = 2,
                AuditName = "Auditorium 1",
                CinemaId = 11,
                Seats = seats1
            };

            List<Auditorium> auditsList = new List<Auditorium>();
            auditsList.Add(audit1);

            var expectedCount = 0;
            _mockSeatRepository.Setup(x => x.GetAll()).ReturnsAsync(seats1);
            _mockCinemaRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_cinema);
            _mockAuditoriumRepository.Setup(x => x.GetAll()).ReturnsAsync(auditsList);

            //Act
            var resultAction = cinemaService.DeleteCinemaAsync(_cinema.Id).ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.AuditoriumsList.Count();

            //Assert
            Assert.AreEqual(expectedCount, result);
        }

        //CreateCinema tests
        [TestMethod]
        public void CinemaService_CreateCinemaAsync_ReturnsListOfCinemas()
        {
            //Arrange
            _city = new City
            {
                Id = 1,
                Name = "Miami"
            };

            _cityDomainModel = new CityDomainModel()
            {
                Id = _city.Id,
                Name = _city.Name
            };

            var numOfRows = 2;
            var numOfSeats = 3;

            _mockCinemaRepository.Setup(x => x.Insert(It.IsAny<Data.Cinema>())).Returns(_cinema);
            _mockCityRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_city);

            //Act
            var resultAction = cinemaService.CreateCinemaAsync(_cinemaDomainModel, numOfSeats, numOfRows).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_cinema.Name, resultAction.Name);
            Assert.IsInstanceOfType(resultAction, typeof(CinemaDomainModel));
        }

        [TestMethod]
        public void CinemaService_CreateCinemaAsync_ReturnsNullCity()
        {
            //Arrange
            _city = new City
            {
                Id = 1,
                Name = "Madrid"
            };

            int numOfRows = 2;
            int numOfSeats = 3;

            _mockCityRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(null as City);
            _mockCinemaRepository.Setup(x => x.Insert(It.IsAny<Data.Cinema>())).Returns(_cinema);

            //Act
            var resultAction = cinemaService.CreateCinemaAsync(_cinemaDomainModel, numOfSeats, numOfRows).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void CinemaService_CreateCinemaAsync_ReturnsListOfCinemasWithAuditoriumsSeats()
        {
            //Arrange
            List<Data.Cinema> cinemas = new List<Data.Cinema>();
           
            _city = new City
            {
                Id = 1,
                Name = "Miami"
            };

            _cityDomainModel = new CityDomainModel()
            {
                Id = _city.Id,
                Name = _city.Name
            };

            SeatDomainModel seat1 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 2,
                Row = 1
            };

            SeatDomainModel seat2 = new SeatDomainModel
            {
                Id = Guid.NewGuid(),
                AuditoriumId = 1,
                Number = 3,
                Row = 1
            };

            Seat s1 = new Seat
            {
                Id = seat1.Id,
                AuditoriumId = seat1.AuditoriumId,
                Number = seat1.Number,
                Row = seat1.Row
            };

            Seat s2 = new Seat
            {
                Id = seat2.Id,
                AuditoriumId = seat2.AuditoriumId,
                Number = seat2.Number,
                Row = seat2.Row
            };

            List<SeatDomainModel> seats = new List<SeatDomainModel>();
            seats.Add(seat1);
            seats.Add(seat2);

            List<Seat> ss = new List<Seat>();
            ss.Add(s1);
            ss.Add(s2);

            AuditoriumDomainModel auditoriumDomainModel = new AuditoriumDomainModel
            {
                Id = 1,
                CinemaId = 11,
                Name = "Test Audit",
                SeatsList = seats
            };

            Auditorium auditorium = new Auditorium
            {
                Id = auditoriumDomainModel.Id,
                CinemaId = auditoriumDomainModel.CinemaId,
                AuditName = auditoriumDomainModel.Name,
                Seats = ss
            };

            List<AuditoriumDomainModel> auditoriumDomainModelList = new List<AuditoriumDomainModel>();
            auditoriumDomainModelList.Add(auditoriumDomainModel);

            List<Auditorium> auditoriumList = new List<Auditorium>();
            auditoriumList.Add(auditorium);

            _cinemaDomainModel.AuditoriumsList = auditoriumDomainModelList;
            _cinema.Auditoriums = auditoriumList;

            int numOfRows = 3;
            int numOfSeats = 2;

            int expectedAuditoriumCount = 1;

            _mockCityRepository.Setup(x => x.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(_city);
            _mockCinemaRepository.Setup(x => x.GetAll()).ReturnsAsync(cinemas);
            _mockCinemaRepository.Setup(x => x.Insert(It.IsAny<Data.Cinema>())).Returns(_cinema);
            _mockCinemaRepository.Setup(x => x.Save());

            //Act
            var resultAction = cinemaService.CreateCinemaAsync(_cinemaDomainModel, numOfSeats, numOfRows).ConfigureAwait(false).GetAwaiter().GetResult();
            

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedAuditoriumCount, resultAction.AuditoriumsList.Count);
            Assert.AreEqual(_cinemaDomainModel.Name, resultAction.Name);
            Assert.IsInstanceOfType(resultAction, typeof(CinemaDomainModel));
        }
    }
}
