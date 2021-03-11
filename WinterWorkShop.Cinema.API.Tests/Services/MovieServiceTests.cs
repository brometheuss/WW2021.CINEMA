using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
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
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(_movie);
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
        public void MovieService_GetAllMovies_ReturnsListOfMovies_QueryYearBiggerThan_ZeroOrLess()
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
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Movie);

            //Act
            var resultAction = movieService.GetMovieByIdAsync(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

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
            Assert.AreEqual(_movie.Id, resultAction.Id);
            Assert.IsInstanceOfType(resultAction, typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_UpdateMovie_ReturnsUpdatedMovie()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.Update(It.IsAny<Movie>())).Returns(_movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.UpdateMovie(_movieDomainModel).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
        }

        //DeleteMovie tests
        [TestMethod]
        public void MovieService_DeleteMovie_ReturnsDeletedMovie()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(_movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.DeleteMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movieDomainModel.Title, resultAction.Title);
            Assert.IsInstanceOfType(resultAction, typeof(MovieDomainModel));
        }

        [TestMethod]
        public void MovieService_DeleteMovie_ReturnsNull()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.Delete(It.IsAny<Guid>())).Returns(null as Movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.DeleteMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }

        //ActivateMovie tests
        [TestMethod]
        public void MovieService_ActivateMovie_ReturnsActivatedMovie()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.ActivateCurrentMovie(It.IsAny<Guid>())).Returns(Task.FromResult(_movie));
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.ActivateMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movieDomainModel.Title, resultAction.Movie.Title);
            Assert.IsInstanceOfType(resultAction, typeof(MovieResultModel));
        }

        [TestMethod]
        public void MovieService_ActivateMovie_ReturnsErrorMessage()
        {
            //Arrange
            MovieResultModel movieResultModel = new MovieResultModel
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                Movie = new MovieDomainModel
                {
                    Current = _movie.Current,
                    HasOscar = _movie.HasOscar,
                    Id = _movie.Id,
                    Rating = _movie.Rating ?? 0,
                    Title = _movie.Title,
                    Year = _movie.Year
                }
            };

            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Movie);
            _mockMovieRepository.Setup(x => x.ActivateCurrentMovie(It.IsAny<Guid>())).ReturnsAsync(null as Movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.ActivateMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(resultAction.IsSuccessful);
            Assert.IsTrue(resultAction.ErrorMessage == movieResultModel.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(MovieResultModel));
        }

        //DeactivateMovie tests
        [TestMethod]
        public void MovieService_DeactivateMovie_ReturnsDeactivatedMovie()
        {
            //Arrange
            MovieResultModel _movieResultModel = new MovieResultModel
            {
                Movie = new MovieDomainModel
                {
                    Title = _movie.Title,
                    Current = _movie.Current,
                    HasOscar = _movie.HasOscar,
                    Id = _movie.Id,
                    Rating = _movie.Rating ?? 0,
                    Year = _movie.Year
                }
            };

            _mockMovieRepository.Setup(x => x.DeactivateCurrentMovie(It.IsAny<Guid>())).ReturnsAsync(_movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.DeactivateMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(_movieResultModel.Movie.Title, resultAction.Movie.Title);
            Assert.IsInstanceOfType(_movieResultModel, typeof(MovieResultModel));
            Assert.IsTrue(resultAction.IsSuccessful);
        }

        [TestMethod]
        public void MovieService_DeactivateMovie_ReturnsErrorMessage()
        {
            //Arrange
            MovieResultModel movieResultModel = new MovieResultModel
            {
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST,
                Movie = new MovieDomainModel
                {
                    Current = _movie.Current,
                    HasOscar = _movie.HasOscar,
                    Id = _movie.Id,
                    Rating = _movie.Rating ?? 0,
                    Title = _movie.Title,
                    Year = _movie.Year
                }
            };
            _mockMovieRepository.Setup(x => x.GetByIdAsync(It.IsAny<Guid>())).ReturnsAsync(null as Movie);
            _mockMovieRepository.Setup(x => x.DeactivateCurrentMovie(It.IsAny<Guid>())).ReturnsAsync(null as Movie);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.DeactivateMovie(_movie.Id).ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsFalse(movieResultModel.IsSuccessful);
            Assert.IsTrue(resultAction.ErrorMessage == movieResultModel.ErrorMessage);
            Assert.IsInstanceOfType(resultAction, typeof(MovieResultModel));
        }

        //GetTopMovies tests
        [TestMethod]
        public void MovieService_GetTopMovies_ReturnsMovieList()
        {
            //Arrange
            var expectedCount = 2;
            Movie movie2 = new Movie
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = false,
                Rating = 3,
                Title = "Jako los muvi",
                Year = 1999
            };
            List<Movie> movieList = new List<Movie>();
            movieList.Add(_movie);
            movieList.Add(movie2);
            _mockMovieRepository.Setup(x => x.GetAll()).ReturnsAsync(movieList);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetTopMovies().ConfigureAwait(false).GetAwaiter().GetResult();
            var result = resultAction.ToList();

            //Assert
            Assert.IsNotNull(resultAction);
            Assert.AreEqual(expectedCount, result.Count);
            Assert.IsInstanceOfType(result, typeof(List<MovieDomainModel>));
            Assert.IsInstanceOfType(resultAction, typeof(IEnumerable<MovieDomainModel>));
        }

        [TestMethod]
        public void MovieService_GetTopMovies_ReturnsNull()
        {
            //Arrange
            _mockMovieRepository.Setup(x => x.GetAll()).ReturnsAsync(null as List<Movie>);
            MovieService movieService = new MovieService(_mockMovieRepository.Object, _mockProjectionRepository.Object);

            //Act
            var resultAction = movieService.GetTopMovies().ConfigureAwait(false).GetAwaiter().GetResult();

            //Assert
            Assert.IsNull(resultAction);
        }
    }
}
