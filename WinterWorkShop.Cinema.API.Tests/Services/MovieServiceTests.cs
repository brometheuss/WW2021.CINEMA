using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Queries;
using WinterWorkShop.Cinema.Domain.Services;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Tests.Services
{
    [TestClass]
    public class MovieServiceTests
    {
        private Mock<IMoviesRepository> _mockMovieRepository;
        private Mock<IProjectionsRepository> _mockProjectionRepository;
        private MovieQuery query;
        private Movie _movie;
        private MovieDomainModel _movieDomainModel;

        [TestInitialize]
        public void TestInitialize()
        {
            _movie = new Movie
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 7,
                Title = "Smekerski film",
                Year = 2021,
                Projections = new List<Projection>()
            };

            _movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                Rating = 7,
                Title = "Smekerski film",
                Year = 2021
            };

            List<Movie> movieModelsList = new List<Movie>();

            movieModelsList.Add(_movie);
            IEnumerable<Movie> movies = movieModelsList;
            Task<IEnumerable<Movie>> responseTask = Task.FromResult(movies);

            query = new MovieQuery
            {
                HasOscar = null
            };
            _mockMovieRepository = new Mock<IMoviesRepository>();
            _mockProjectionRepository = new Mock<IProjectionsRepository>();
            _mockMovieRepository.Setup(x => x.GetCurrentMovies()).Returns(movieModelsList);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies()
        {
            //Arrange
            int expectedCount = 1;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);
            //query.Title = "Smekerski film";


            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsNull()
        {
            //Arrange
            IEnumerable<Movie> movies = null;
            Task<IEnumerable<Movie>> responseTask = Task.FromResult(movies);
            _mockMovieRepository.Setup(x => x.GetCurrentMovies()).Returns(movies);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);

            //Assert
            Assert.IsNull(resultAction);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryTitle_NotNull()
        {
            //Arrange
            int expectedCount = 1;
            query.Title = "Smekerski film";
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryTitle_IsNull()
        {
            //Arrange
            int expectedCount = 1;
            query.Title = null;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryRatingLowerThan_ZeroOrLess()
        {
            //Arrange
            var expectedCount = 1;
            query.RatingLowerThan = 0;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryRatingLowerThan_BiggerThanZero()
        {
            //Arrange
            var exptectedCount = 1;
            query.RatingLowerThan = 8;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exptectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryRatingBiggerThan_ZeroOrLess()
        {
            //Arrange
            var expectedCount = 1;
            query.RatingBiggerThan = 0;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryRatingBiggerThan_BiggerThanZero()
        {
            //Arrange
            var expectedCount = 1;
            query.RatingBiggerThan = 0;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }
    }
}
