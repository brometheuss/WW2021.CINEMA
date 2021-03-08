using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Interfaces;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Queries;
using WinterWorkShop.Cinema.Repositories;

namespace WinterWorkShop.Cinema.Domain.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMoviesRepository _moviesRepository;
        private readonly IProjectionsRepository _projectionRepository;

        public MovieService(IMoviesRepository moviesRepository, IProjectionsRepository projectionRepository)
        {
            _moviesRepository = moviesRepository;
            _projectionRepository = projectionRepository;
        }

        public IEnumerable<MovieDomainModel> GetAllMovies(bool? isCurrent, MovieQuery query)
        {
            var data = _moviesRepository.GetCurrentMovies();

            if (query.ActorName != null)
                data = data.Where(x => x.MovieActors.Any(a => a.Actor.FirstName.Contains(query.ActorName.ToLower())));

            if (query.Title != null)
                data = data.Where(x => x.Title.ToLower().Contains(query.Title.ToLower()));

            if (query.RatingLowerThan > 0)
                data = data.Where(x => x.Rating < query.RatingLowerThan);

            if (query.RatingBiggerThan > 0)
                data = data.Where(x => x.Rating > query.RatingBiggerThan);

            if (query.YearLowerThan > 0)
                data = data.Where(x => x.Year < query.YearLowerThan);

            if (query.YearBiggerThan > 0)
                data = data.Where(x => x.Year > query.YearBiggerThan);

            if (query.HasOscar != null)
                data = data.Where(x => x.HasOscar == query.HasOscar);

            if (data == null)
            {
                return null;
            }

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;
            foreach (var item in data)
            {
                model = new MovieDomainModel
                {
                    Current = item.Current,
                    Id = item.Id,
                    Rating = item.Rating ?? 0,
                    Title = item.Title,
                    Year = item.Year
                };
                result.Add(model);
            }

            return result;

        }

        public async Task<MovieDomainModel> GetMovieByIdAsync(Guid id)
        {
            var data = await _moviesRepository.GetByIdAsync(id);

            if (data == null)
            {
                return null;
            }

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

        public async Task<MovieDomainModel> AddMovie(MovieDomainModel newMovie)
        {
            Movie movieToCreate = new Movie()
            {
                Title = newMovie.Title,
                Current = newMovie.Current,
                Year = newMovie.Year,
                Rating = newMovie.Rating
            };

            var data = _moviesRepository.Insert(movieToCreate);
            if (data == null)
            {
                return null;
            }

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = data.Id,
                Title = data.Title,
                Current = data.Current,
                Year = data.Year,
                Rating = data.Rating ?? 0
            };

            return domainModel;
        }

        public async Task<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie) {

            Movie movie = new Movie()
            {
                Id = updateMovie.Id,
                Title = updateMovie.Title,
                Current = updateMovie.Current,
                Year = updateMovie.Year,
                Rating = updateMovie.Rating
            };
            
            var data = _moviesRepository.Update(movie);

            if (data == null)
            {
                return null;
            }
            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel()
            {
                Id = data.Id,
                Title = data.Title,
                Current = data.Current,
                Year = data.Year,
                Rating = data.Rating ?? 0
            };

            return domainModel;
        }

        public async Task<MovieDomainModel> DeleteMovie(Guid id)
        {
            var data = _moviesRepository.Delete(id);

            if (data == null)
            {
                return null;
            }

            _moviesRepository.Save();

            MovieDomainModel domainModel = new MovieDomainModel
            {
                Id = data.Id,
                Title = data.Title,
                Current = data.Current,
                Year = data.Year,
                Rating = data.Rating ?? 0

            };
            
            return domainModel;
        }

        public async Task<MovieDomainModel> DeactivateMovie(Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return null;
            }

            var projections = await _projectionRepository.GetAll();

            var futureProjections = projections.Where(p => p.MovieId == id && p.DateTime > DateTime.Now);
            if(futureProjections.Count() > 0)
            {
                return null;
            }

            var deactivatedMovie = await _moviesRepository.DeactivateCurrentMovie(movie.Id);
            _moviesRepository.Save();

            MovieDomainModel deactivatedModel = new MovieDomainModel
            {
                Id = movie.Id,
                Current = deactivatedMovie.Current,
                Rating = movie.Rating ?? 0,
                Title = movie.Title,
                Year = movie.Year
            };

            return deactivatedModel;
        }

        public async Task<MovieDomainModel> ActivateMovie(Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);
            
            if(movie == null)
            {
                return null;
            }

            var activatedMovie = await _moviesRepository.ActivateCurrentMovie(movie.Id);
            _moviesRepository.Save();

            MovieDomainModel activatedModel = new MovieDomainModel
            {
                Id = movie.Id,
                Current = activatedMovie.Current,
                Rating = movie.Rating ?? 0,
                Title = movie.Title,
                Year = movie.Year
            };

            return activatedModel;
        }

        public async Task<IEnumerable<MovieDomainModel>> GetTopMovies()
        {
            var movies = await _moviesRepository.GetAll();

            if(movies == null)
            {
                return null;
            }
            movies = movies.Where(movie => movie.Current == true);
            movies = movies.OrderByDescending(movie => movie.Rating);

            List<MovieDomainModel> result = new List<MovieDomainModel>();
            MovieDomainModel model;
            foreach (var item in movies)
            {
                model = new MovieDomainModel
                {
                    Current = item.Current,
                    Id = item.Id,
                    Rating = item.Rating ?? 0,
                    Title = item.Title,
                    Year = item.Year
                };
                result.Add(model);
            }

            return result;
        }
    }
}
