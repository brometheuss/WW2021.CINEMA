
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.API.Controllers;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Domain.Common;
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
        public void GetAllMovies_Async_Return_All_Movies_Non_Current_Included()
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
        public void GetAllMovies_Async_Return_All_Movies_Non_Current_Included_NewEmptyList()
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
        [TestMethod]
        public void PatchAsync_ChangeCurrent_IsSuccessful_Current_Field_Changed()
        {
            //Arrange
            MovieDomainModel movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 2,
                Title = "Film1",
                Year = 2015
            };

            MovieResultModel movieResultModel = new MovieResultModel
            {
                Movie = movieDomainModel,
                IsSuccessful = true,
                ErrorMessage = null
            };
            int expectedStatusCode = 202;
            Task<MovieResultModel> responseTask = Task.FromResult(movieResultModel);
            Task<MovieDomainModel> responseTask2 = Task.FromResult(movieDomainModel);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetMovieByIdAsync(It.IsAny<Guid>())).Returns(responseTask2);
            _movieService.Setup(x => x.Activate_DeactivateMovie(It.IsAny<Guid>())).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.ChangeCurrent(movieDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((AcceptedResult)result).Value;
            var movieDomainModelResult = (MovieDomainModel)objectResult;

            //Assert
            Assert.IsNotNull(movieDomainModelResult);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
        }

        [TestMethod]
        public void PatchAsync_MovieDoesNotExist_Return_BadRequest()
        {
            //Arrange
            string expectedMessage = Messages.MOVIE_DOES_NOT_EXIST;
            int expectedStatusCode = 400;

            MovieDomainModel movieDomainModel = null;
            Task<MovieDomainModel> responseTask = Task.FromResult(movieDomainModel);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetMovieByIdAsync(It.IsAny<Guid>())).Returns(responseTask);

            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.ChangeCurrent(Guid.NewGuid()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = (ErrorResponseModel)objectResult;

            //Assert
            Assert.IsNotNull(errorResponse);
            Assert.AreEqual(expectedMessage, errorResponse.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void PatchAsync_DbUpdateException_Throw()
        {
            //Arrange
            MovieDomainModel movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 2,
                Title = "Film1",
                Year = 2015
            };
            string expectedMessage = "Inner exception error message.";
            int expectedStatusCode = 400;
            Exception exception = new Exception("Inner exception error message.");
            DbUpdateException dbUpdateException = new DbUpdateException("Error.", exception);
            Task<MovieDomainModel> responseTask = Task.FromResult(movieDomainModel);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetMovieByIdAsync(It.IsAny<Guid>())).Returns(responseTask);
            _movieService.Setup(x => x.Activate_DeactivateMovie(It.IsAny<Guid>())).Throws(dbUpdateException);

            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.ChangeCurrent(Guid.NewGuid()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = (ErrorResponseModel)objectResult;

            //Assert
            Assert.IsNotNull(errorResponse);
            Assert.AreEqual(expectedMessage, errorResponse.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void PatchAsync_IsNot_Successful_Movie_Cannot_Be_Patched_Return_BadRequest()
        {
            //Arrange
            MovieDomainModel movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 2,
                Title = "Film1",
                Year = 2015
            };

            MovieResultModel movieResultModel = new MovieResultModel
            {
                Movie = movieDomainModel,
                IsSuccessful = false,
                ErrorMessage = Messages.MOVIE_DEACTIVATION_ERROR
            };

            string expectedMessage = "Cannot deactivate movie which has future projections";
            int expectedStatusCode = 400;

            Task<MovieDomainModel> responseTask1 = Task.FromResult(movieDomainModel);
            Task<MovieResultModel> responseTask2 = Task.FromResult(movieResultModel);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetMovieByIdAsync(It.IsAny<Guid>())).Returns(responseTask1);
            _movieService.Setup(x => x.Activate_DeactivateMovie(It.IsAny<Guid>())).Returns(responseTask2);

            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.ChangeCurrent(Guid.NewGuid()).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((BadRequestObjectResult)result).Value;
            var errorResponse = (ErrorResponseModel)objectResult;

            //Assert
            Assert.IsNotNull(errorResponse);
            Assert.AreEqual(expectedMessage, errorResponse.ErrorMessage);
            Assert.AreEqual(expectedStatusCode, ((BadRequestObjectResult)result).StatusCode);
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
        }

        [TestMethod]
        public void GetCurrentMoviesAndProjections_Return_All()
        {
            //Arrange
            int expectedCount = 1;
            int expectedStatusCode = 200;

            List<MovieProjectionDomainModel> movieProjectionDomainModels = new List<MovieProjectionDomainModel>();

            MovieProjectionDomainModel movieProjectionDomainModel = new MovieProjectionDomainModel
            {
                Id = Guid.NewGuid(),
                Rating = 5,
                Title = "film",
                Year = 2005,
                Projections = new List<ProjectionDomainModel>()
            };
            movieProjectionDomainModels.Add(movieProjectionDomainModel);
            IEnumerable<MovieProjectionDomainModel> movieProjectionDomainModelsIEn = movieProjectionDomainModels;
            Task<IEnumerable<MovieProjectionDomainModel>> responseTask = Task.FromResult(movieProjectionDomainModelsIEn);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetCurrentMoviesAndProjections()).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.GetCurrentMoviesAndProjections().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieProjectionDomainModelResultList = (List<MovieProjectionDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedCount, movieProjectionDomainModelResultList.Count);
            Assert.AreEqual(movieProjectionDomainModel.Id, movieProjectionDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }
        [TestMethod]
        public void GetCurrentMoviesAndProjections_Return_EmptyList()
        {
            //Arrange
            int expectedCount = 0;
            int expectedStatusCode = 200;

            IEnumerable<MovieProjectionDomainModel> movieProjectionDomainModels = null;

           
            Task<IEnumerable<MovieProjectionDomainModel>> responseTask = Task.FromResult(movieProjectionDomainModels);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetCurrentMoviesAndProjections()).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.GetCurrentMoviesAndProjections().ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieProjectionDomainModelResultList = (List<MovieProjectionDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedCount, movieProjectionDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void GetMoviesByAuditoriumId_ReturnAll()
        {
            //Arrange
            int expectedCount = 1;
            int expectedStatusCode = 200;
            int auditoriumId = 25;

            MovieDomainModel movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 2,
                Title = "Film1",
                Year = 2015
            };

            List<MovieDomainModel> movieDomainModels = new List<MovieDomainModel>();

            
            movieDomainModels.Add(movieDomainModel);
            IEnumerable<MovieDomainModel> movieDomainModelsIEn = movieDomainModels;
            Task<IEnumerable<MovieDomainModel>> responseTask = Task.FromResult(movieDomainModelsIEn);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetMoviesByAuditoriumId(It.IsAny<int>())).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.GetMoviesByAuditoriumId(auditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieDomainModelResultList = (List<MovieDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedCount, movieDomainModelResultList.Count);
            Assert.AreEqual(movieDomainModel.Id, movieDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);

        }

        [TestMethod]
        public void GetMoviesByAuditoriumId_Return_EmptyList()
        {
            //Arrange
            int expectedCount = 0;
            int expectedStatusCode = 200;
            int auditoriumId = 25;

            IEnumerable<MovieDomainModel> movieDomainModelsIEn = null;
            Task<IEnumerable<MovieDomainModel>> responseTask = Task.FromResult(movieDomainModelsIEn);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetMoviesByAuditoriumId(It.IsAny<int>())).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.GetMoviesByAuditoriumId(auditoriumId).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieDomainModelResultList = (List<MovieDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedCount, movieDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void TopByYear_Return_All()
        {
            //Arrange
            int expectedCount = 1;
            int expectedStatusCode = 200;
            int year = 25;

            MovieDomainModel movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 2,
                Title = "Film1",
                Year = 2015
            };

            List<MovieDomainModel> movieDomainModels = new List<MovieDomainModel>();


            movieDomainModels.Add(movieDomainModel);
            IEnumerable<MovieDomainModel> movieDomainModelsIEn = movieDomainModels;
            Task<IEnumerable<MovieDomainModel>> responseTask = Task.FromResult(movieDomainModelsIEn);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetTopByYear(It.IsAny<int>())).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.TopByYear(year).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieDomainModelResultList = (List<MovieDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedCount, movieDomainModelResultList.Count);
            Assert.AreEqual(movieDomainModel.Id, movieDomainModelResultList[0].Id);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void TopByYear_Return_EmptyList()
        {
            //Arrange
            int expectedCount = 0;
            int expectedStatusCode = 200;
            int year = 25;

            IEnumerable<MovieDomainModel> movieDomainModelsIEn = null;
            Task<IEnumerable<MovieDomainModel>> responseTask = Task.FromResult(movieDomainModelsIEn);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.GetTopByYear(It.IsAny<int>())).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.TopByYear(year).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var resultList = ((OkObjectResult)result).Value;
            var movieDomainModelResultList = (List<MovieDomainModel>)resultList;

            //Assert
            Assert.IsNotNull(resultList);
            Assert.AreEqual(expectedCount, movieDomainModelResultList.Count);
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            Assert.AreEqual(expectedStatusCode, ((OkObjectResult)result).StatusCode);
        }

        [TestMethod]
        public void Delete_Successful_Return_Accepted()
        {
            int expectedStatusCode = 202;

            MovieDomainModel movieDomainModel = new MovieDomainModel
            {
                Id = Guid.NewGuid(),
                Current = true,
                HasOscar = true,
                Rating = 2,
                Title = "Film1",
                Year = 2015
            };

            Task<MovieDomainModel> responseTask = Task.FromResult(movieDomainModel);

            _movieService = new Mock<IMovieService>();
            _loggerService = new Mock<ILogger<MoviesController>>();
            _movieService.Setup(x => x.DeleteMovie(It.IsAny<Guid>())).Returns(responseTask);
            MoviesController moviesController = new MoviesController(_loggerService.Object, _movieService.Object);

            //Act
            var result = moviesController.Delete(movieDomainModel.Id).ConfigureAwait(false).GetAwaiter().GetResult().Result;
            var objectResult = ((AcceptedResult)result).Value;
            var movieDomainModelResult = (MovieDomainModel)objectResult;

            //Assert
            Assert.IsNotNull(movieDomainModel);
            Assert.IsInstanceOfType(result, typeof(AcceptedResult));
            Assert.AreEqual(expectedStatusCode, ((AcceptedResult)result).StatusCode);
        }


    }
}
