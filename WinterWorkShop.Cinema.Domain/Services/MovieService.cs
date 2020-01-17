using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<MovieDomainModel> GetMovieByIdAsync(Guid id)
        {
            var data = await _moviesRepository.GetByIdAsync(id);

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = data.Id,
                Current = data.Current,
                Rating = data.Rating ?? 0,
                Title = data.Title,
                Year = data.Year                
            };

            return domainModel;
        }

        public MovieDomainModel UpdateMovie(MovieDomainModel updateMovie) {

            Movie movie = new Movie()
            {
                Id = updateMovie.Id,
                Title = updateMovie.Title,
                Current = updateMovie.Current,
                Year = updateMovie.Year,
                Rating = updateMovie.Rating
            };


            var data = _moviesRepository.Update(movie);
            _moviesRepository.Save();


            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = data.Entity.Id,
                Title = data.Entity.Title,
                Current = data.Entity.Current,
                Year = data.Entity.Year,
                Rating = data.Entity.Rating ?? 0
            };

            return domainModel;
        }

        public MovieDomainModel DeleteMovie(Guid id)
        {
            var data = _moviesRepository.Delete(id);
            if (data == null)
            {
                return null;
            }

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = data.Entity.Id,
                Title = data.Entity.Title,
                Current = data.Entity.Current,
                Year = data.Entity.Year,
                Rating = data.Entity.Rating ?? 0

            };
            
            return domainModel;
        }
    }
}
