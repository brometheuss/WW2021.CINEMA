using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMoviesRepository _moviesRepository;

        public MovieService(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        public MovieDomainModel AddMovie(MovieDomainModel newMovie)
        {
            var data = _moviesRepository.Insert(new Movie
            {
                Title = newMovie.Title,
                Current = newMovie.Current,
                Year = newMovie.Year,
                Rating = newMovie.Rating
            });

            _moviesRepository.Save();

            MovieDomainModel movie = new MovieDomainModel
            {
                Id = data.Entity.Id,
                Current = data.Entity.Current,
                Rating = data.Entity.Rating ?? 0,
                Title = data.Entity.Title,
                Year = data.Entity.Year
            };

            return movie;
        }

        public IEnumerable<MovieDomainModel> GetAllMovies(bool? isCurrent)
        {
            var data = _moviesRepository.GetCurrentMovies();

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;
            foreach (var item in data)
            {
                model = new MovieDomainModel
                {
                    Current = item.Current,
                    Id = item.Id,
                    Rating = item.Rating ?? 0,
                    Title = item.Title
                };
                result.Add(model);
            }

            return result;
        }

        public MovieDomainModel GetMovieById(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
