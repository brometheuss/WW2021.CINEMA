using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class ProjectionsServiceTests
    {
        private Mock<IProjectionsRepository> _projectionsRepository;


        [TestMethod]
        public void ProjectionService_GetAllAsync_ReturnListOfProjecrions()
        {
            //Arrange
            List<Projection> projectionsModelsList = new List<Projection>();
            Projection projection = new Projection
            {
                Id = Guid.NewGuid(),
                Auditorium = new Auditorium { AuditName = "ImeSale" },
                Movie = new Movie { Title = "ImeFilma" },
                MovieId = Guid.NewGuid(),
                DateTime = DateTime.Now.AddDays(1),
                SalaId = 1
            };
            projectionsModelsList.Add(projection);
            IEnumerable<Projection> projections = projectionsModelsList;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);
            int expectedResultCount = 1;

            _projectionsRepository = new Mock<IProjectionsRepository>();
            _projectionsRepository.Setup(x => x.GetAll()).Returns(responseTask);
            ProjectionService projectionsController = new ProjectionService(_projectionsRepository.Object);

            //Act
            var resultAction = projectionsController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = (List<ProjectionDomainModel>)resultAction;

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResultCount, result.Count);
            Assert.AreEqual(projection.Id, result[0].Id);
            Assert.IsInstanceOfType(result[0], typeof(ProjectionDomainModel));
        }

        [TestMethod]
        public void ProjectionService_GetAllAsync_ReturnNull()
        {
            //Arrange
            IEnumerable<Projection> projections = null;
            Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);

            _projectionsRepository = new Mock<IProjectionsRepository>();
            _projectionsRepository.Setup(x => x.GetAll()).Returns(responseTask);
            ProjectionService projectionsController = new ProjectionService(_projectionsRepository.Object);

            //Act
            var resultAction = projectionsController.GetAllAsync().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        // _projectionsRepository.GetBySalaId(domainModel.AuditoriumId) mocked to return list with projections
        // if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0) - true
        // return ErrorMessage
        [TestMethod]
        public void ProjectionService_CreateProjection_WithProjectionAtSameTime_ReturnErrorMessage() 
        {
            //Arrange
            ProjectionDomainModel projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };

            List<Projection> projectionsModelsList = new List<Projection>();
            Projection projection = new Projection
            {
                Id = Guid.NewGuid(),
                Auditorium = new Auditorium { AuditName = "ImeSale" },
                Movie = new Movie { Title = "ImeFilma" },
                MovieId = Guid.NewGuid(),
                DateTime = DateTime.Now.AddDays(1),
                SalaId = 1
            };
            projectionsModelsList.Add(projection);
            //IEnumerable<Projection> projections = projectionsModelsList;
            //Task<IEnumerable<Projection>> responseTask = Task.FromResult(projections);
            bool expectedResult = false;
            string expectedMessage = "Cannot create new projection, there are projections at same time alredy.";

            _projectionsRepository = new Mock<IProjectionsRepository>();
            _projectionsRepository.Setup(x => x.GetBySalaId(It.IsAny<int>())).Returns(projectionsModelsList);
            ProjectionService projectionsController = new ProjectionService(_projectionsRepository.Object);

            //Act
            var resultAction = projectionsController.CreateProjection(projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedMessage, resultAction.ErrorMessage);
            Assert.AreEqual(expectedResult, resultAction.IsSuccessful);
        }

        // _projectionsRepository.GetBySalaId(domainModel.AuditoriumId) mocked to return empty list
        // if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0) - false
        // _projectionsRepository.Insert(newProjection) mocked to return null
        //  if (insertedProjection == null) - true
        // return null 
        [TestMethod]
        public void ProjectionService_CreateProjection_InsertMocked_ReturnNull()
        {
            //Arrange
            ProjectionDomainModel projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };
            List<Projection> projectionsModelsList = new List<Projection>();
            Projection projection = null;

            _projectionsRepository = new Mock<IProjectionsRepository>();
            _projectionsRepository.Setup(x => x.GetBySalaId(It.IsAny<int>())).Returns(projectionsModelsList);
            _projectionsRepository.Setup(x => x.Insert(It.IsAny<Projection>())).Returns(projection);
            ProjectionService projectionsController = new ProjectionService(_projectionsRepository.Object);

            //Act
            var resultAction = projectionsController.CreateProjection(projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        // _projectionsRepository.GetBySalaId(domainModel.AuditoriumId) mocked to return empty list
        // if (projectionsAtSameTime != null && projectionsAtSameTime.Count > 0) - false
        // _projectionsRepository.Insert(newProjection) mocked to return valid EntityEntry<Projection>
        //  if (insertedProjection == null) - false
        // return valid projection 
        [TestMethod]
        public void ProjectionService_CreateProjection_InsertMocked_ReturnProjection()
        {
            //Arrange
            ProjectionDomainModel projectionDomainModel = new ProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                AditoriumName = "ImeSale",
                AuditoriumId = 1,
                MovieId = Guid.NewGuid(),
                MovieTitle = "ImeFilma",
                ProjectionTime = DateTime.Now.AddDays(1)
            };
            List<Projection> projectionsModelsList = new List<Projection>();
            Projection projection = new Projection
            {
                Id = Guid.NewGuid(),
                Auditorium = new Auditorium { AuditName = "ImeSale" },
                Movie = new Movie { Title = "ImeFilma" },
                MovieId = Guid.NewGuid(),
                DateTime = DateTime.Now.AddDays(1),
                SalaId = 1
            };


            _projectionsRepository = new Mock<IProjectionsRepository>();
            _projectionsRepository.Setup(x => x.GetBySalaId(It.IsAny<int>())).Returns(projectionsModelsList);
            _projectionsRepository.Setup(x => x.Insert(It.IsAny<Projection>())).Returns(projection);
            _projectionsRepository.Setup(x => x.Save());
            ProjectionService projectionsController = new ProjectionService(_projectionsRepository.Object);

            //Act
            var resultAction = projectionsController.CreateProjection(projectionDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
        }
    }
}
