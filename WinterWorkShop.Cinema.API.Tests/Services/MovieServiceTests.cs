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
        private Movie _movie2;
        private MovieDomainModel _movieDomainModel;
        private MovieDomainModel _movieDomainModel2;

        [TestInitialize]
        public void TestInitialize()
        {
            List<Projection> projections = new List<Projection>();
            
            _movie = new Movie
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 7,
                Title = "Smekerski film",
                Year = 2021,
                Projections = projections
            };

            _movieDomainModel = new MovieDomainModel
            {
                Id = _movie.Id,
                Current = _movie.Current,
                HasOscar = _movie.HasOscar,
                Rating = _movie.Rating ?? 0,
                Title = _movie.Title,
                Year = _movie.Year
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
            _mockMovieRepository.Setup(x => x.Insert(It.IsAny<Movie>())).Returns(_movie);
        }

        //GetAllMovies tests
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
            Assert.AreEqual(_movie.Title, result[0].Title);
            Assert.AreEqual(_movie.Rating, result[0].Rating);
            Assert.AreEqual(_movie.Year, result[0].Year);
            Assert.IsInstanceOfType(result[0], typeof(MovieDomainModel));
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
            Assert.AreEqual(_movie.Title, result[0].Title);
            Assert.AreEqual(_movie.Rating, result[0].Rating);
            Assert.AreEqual(_movie.Year, result[0].Year);
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

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryYearLowerThan_ZeroOrLess()
        {
            //Arrange
            var expectedCount = 1;
            query.YearLowerThan = 0;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryYearLowerThan_BiggerThanZero()
        {
            //Arrange
            var exptectedCount = 0;
            query.YearLowerThan = 1900;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(exptectedCount, result.Count);
        }

        [TestMethod]
        public  void MovieService_GetAllMovies_ReturnsListOfMovies_QueryYearBiggerThan_ZeroOrLess()
        {
            //Arrange
            var expectedCount = 1;
            query.YearBiggerThan = 0;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryYearBiggerThan_BiggerThanZero()
        {
            //Arrange
            var expectedCount = 1;
            query.YearBiggerThan = 1900;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryHasOscarFalse()
        {
            //Arrange
            var expectedCount = 0;
            query.HasOscar = false;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
        }

        [TestMethod]
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryHasOscarTrue()
        {
            //Arrange
            var expectedCount = 1;
            query.HasOscar = true;
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetAllMovies(true, query);
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result[0], typeof(MovieDomainModel));
        }


        //GetMovieById tests
        [TestMethod]
        public void MovieService_GetMovieByIdAsync_ReturnsMovie()
        {
            //Arrange
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);

            //Act
            var resultAction = movieService.GetMovieByIdAsync(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();


            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movie.Title, resultAction.Title);
            Assert.IsInstanceOfType(resultAction, typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_GetMovieByIdAsync_ReturnsNull()
        {
            //Arrange 
            _movie2 = new Movie();
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);

            //Act
            var resultAction = movieService.GetMovieByIdAsync(_movie2.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //CreateMovie tests
        [TestMethod]
        public void MovieService_AddMovie_ReturnsCreatedMovie()
        {
            //Arrange
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.AddMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movieDomainModel.Title, resultAction.Title);
        }
    }
}
