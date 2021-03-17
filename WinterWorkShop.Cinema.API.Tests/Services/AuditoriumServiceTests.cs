using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data;
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

            List<Auditorium> auditoriums = new List<Auditorium>();
            auditoriums.Add(_auditorium);

            _cinema = new Data.Cinema
            {
                Id = 1,
                CityId = 1,
                Name = "test bioskop 1",
                Auditoriums = auditoriums
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
    }
}
