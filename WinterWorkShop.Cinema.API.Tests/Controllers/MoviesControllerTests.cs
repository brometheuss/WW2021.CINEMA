
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Tests.Controllers
{
    [TestClass]
    public class MoviesControllerTests
    {
        private Mock<IMovieService> _movieService;
        private Mock<ILogger<MoviesController>> _loggerService;

        [TestMethod]
        public void GetAsync_Return_All_Movies_Non_Current_Included()
        {
            //Arrange
            List<MovieDomainModel> moviesDomainModelsList = new List<MovieDomainModel>();

            MovieDomainModel movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 2,
                Title = "Film1",
                Year = 2015
            };

            moviesDomainModelsList.Add(movieDomainModel);
            IEnumerable<MovieDomainModel> movieDomainModels = moviesDomainModelsList;

            Task<IEnumerable<MovieDomainModel>> responseTask = Task
                .FromResult(movieDomainModels);

            int expectedResultCount = 1;
            int expectedStatusCode = 200;

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();

            _movieService.Setup(x => x.GetAllMoviesNonCurrentIncluded()).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.GetAllMovies().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieDomainModelResultList = (List<MovieDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(movieDomainModelResultList);
            Assert.AreEqual(expectedResultCount, movieDomainModelResultList.Count);
            Assert.AreEqual(movieDomainModel.Id, movieDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetAsync_Return_All_Movies_Non_Current_Included_NewEmptyList()
        {
            //Arrange
            
            IEnumerable<MovieDomainModel> movieDomainModels = null;

            Task<IEnumerable<MovieDomainModel>> responseTask = Task
                .FromResult(movieDomainModels);

            int expectedResultCount = 0;
            int expectedStatusCode = 200;

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();

            _movieService.Setup(x => x.GetAllMoviesNonCurrentIncluded()).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.GetAllMovies().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieDomainModelResultList = (List<MovieDomainModel>)resultList;

            
            //Assert
            Assert.IsNotNull(movieDomainModelResultList);
            Assert.AreEqual(expectedResultCount, movieDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
    }
}
