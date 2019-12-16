using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WinterWorkShop.Cinema.API.Models;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MoviesController : ControllerBase
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly CinemaContext _cinemaContext;

        private readonly ILogger<MoviesController> _logger;

        public MoviesController(ILogger<MoviesController> logger, CinemaContext cinemaContext, IMoviesRepository moviesRepository)
        {
            _logger = logger;
            _moviesRepository = moviesRepository;
            _cinemaContext = cinemaContext;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAsync(Guid id)
        {
            var data = _moviesRepository.GetById(id);
            return Ok(data);
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetAsync()
        {
            var data = await _moviesRepository.GetAll();
            return Ok(data);
        }

        [HttpPost]
        public async Task<ActionResult> Post(MovieModel movieModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var data = _moviesRepository.Insert(new Movie
            {
                Title = movieModel.Title,
                Current = movieModel.Current,
                Year = movieModel.Year,
                Rating = movieModel.Rating
            });

            _moviesRepository.Save();

            return Created("movies//" + data.Entity.Id, data.Entity);
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
