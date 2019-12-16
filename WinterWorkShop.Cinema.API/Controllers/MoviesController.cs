using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly CinemaContext _cinemaContext;
        private readonly IMovieService _movieService;

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger, IMovieService movieService)
        {
            _logger = logger;
            _movieService = movieService;            
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAsync(Guid id)
        {
            var data = _moviesRepository.GetById(id);
            return Ok(data);
        }

        [HttpGet]
        [Route("current")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAsync()
        {
            var data = _movieService.GetAllMovies(true);
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Current = movieModel.Current,
                Rating = movieModel.Rating,
                Title = movieModel.Title,
                Year = movieModel.Year
            };

            var data = _movieService.AddMovie(domainModel);

            return Created("movies//" + data.Id, data);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult> Put(Guid id, [FromBody]MovieModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var movieToUpdate = _moviesRepository.GetById(id);

            movieToUpdate.Title = movieModel.Title;
            movieToUpdate.Current = movieModel.Current;
            movieToUpdate.Year = movieModel.Year;
            movieToUpdate.Rating = movieModel.Rating;

            _moviesRepository.Update(movieToUpdate);

            _moviesRepository.Save();

            return Accepted("movies//" + movieToUpdate.Id, movieToUpdate);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult> Delete(Guid id)
        {
            var deletedMovie = _moviesRepository.Delete(id);
            _moviesRepository.Save();

            return Accepted("movies//" + deletedMovie.Entity.Id, deletedMovie.Entity);
        }
    }
}
