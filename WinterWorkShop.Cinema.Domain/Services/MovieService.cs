using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Domain.Common;
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
                data = data.Where(x => x.MovieActors.Any(a => a.Actor.FirstName.ToLower().Contains(query.ActorName.ToLower())));

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
                Year = data.Year,
                HasOscar = data.HasOscar
            };

            return domainModel;
        }

        public async Task<MovieDomainModel> AddMovie(MovieDomainModel newMovie)
        {
            Movie movieToCreate = new Movie()
            {
                Id = Guid.NewGuid(),
                Title = newMovie.Title,
                Current = newMovie.Current,
                Year = newMovie.Year,
                Rating = newMovie.Rating,
                HasOscar = newMovie.HasOscar
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
                Rating = data.Rating ?? 0,
                HasOscar = data.HasOscar
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
                Rating = updateMovie.Rating,
                HasOscar = updateMovie.HasOscar
                
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
                Rating = data.Rating ?? 0,
                HasOscar = data.HasOscar
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

        public async Task<MovieResultModel> DeactivateMovie(Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);
            if (movie == null)
            {
                return new MovieResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }

            var projections = await _projectionRepository.GetAll();

            var futureProjections = projections.Where(p => p.MovieId == id && p.DateTime > DateTime.Now);
            if(futureProjections.Count() > 0)
            {
                return new MovieResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DEACTIVATION_ERROR,
                };
            }

            var deactivatedMovie = await _moviesRepository.DeactivateCurrentMovie(movie.Id);
            _moviesRepository.Save();

       
            MovieResultModel movieResultModel = new MovieResultModel
            {
                Movie = new MovieDomainModel
                {
                    Id = movie.Id,
                    Current = deactivatedMovie.Current,
                    Rating = movie.Rating ?? 0,
                    Title = movie.Title,
                    Year = movie.Year,
                    HasOscar = movie.HasOscar
                },
                IsSuccessful = true,
                ErrorMessage = null

            };
            

            return movieResultModel;
        }

        public async Task<MovieResultModel> ActivateMovie(Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);
            
            if(movie == null)
            {
                return new MovieResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }

            var activatedMovie = await _moviesRepository.ActivateCurrentMovie(movie.Id);
            _moviesRepository.Save();

            MovieDomainModel activatedModel = new MovieDomainModel
            {
                Id = movie.Id,
                Current = activatedMovie.Current,
                Rating = movie.Rating ?? 0,
                Title = movie.Title,
                Year = movie.Year,
                HasOscar = movie.HasOscar
            };

            return new MovieResultModel
            {
                IsSuccessful = true,
                Movie = activatedModel
            };
        }

        public async Task<IEnumerable<MovieDomainModel>> GetTopMovies()
        {
            var movies = await _moviesRepository.GetAll();

            if(movies == null)
            {
                return null;
            }
            movies = movies.Where(movie => movie.Current == true);
            movies = movies.OrderByDescending(movie => movie.Rating).Take(10);

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

        public async Task<IEnumerable<MovieDomainModel>> GetTopByYear(int year)
        {
            var movies = await _moviesRepository.GetAll();

            if(movies == null)
            {
                return null;
            }

            movies = movies.Where(m => m.Year == year).Take(10);

            List<MovieDomainModel> movieModels = movies.Select(m => new MovieDomainModel
            {
                Current = m.Current,
                Id = m.Id,
                Rating = m.Rating ?? 0,
                Title = m.Title,
                Year = m.Year
            }).ToList();

            return movieModels; 
        }

        public async Task<IEnumerable<MovieDomainModel>> GetAllMoviesNonCurrentIncluded()
        {
            var movies = await _moviesRepository.GetAll();

            if(movies.Count() == 0)
            {
                return null;
            }

            IEnumerable<MovieDomainModel> listMovies = new List<MovieDomainModel>();

            listMovies = movies.Select(movie => new MovieDomainModel
            {
                Id = movie.Id,
                Current = movie.Current,
                HasOscar = movie.HasOscar,
                Rating = movie.Rating ?? 0,
                Title = movie.Title,
                Year = movie.Year
            });

            return listMovies;
        }

        public async Task<MovieResultModel> Activate_DeactivateMovie(Guid id)
        {
            var movie = await _moviesRepository.GetByIdAsync(id);

            if(movie == null)
            {
                return new MovieResultModel
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.MOVIE_DOES_NOT_EXIST
                };
            }

            if(movie.Current == false)
            {
                var activatedMovie = await _moviesRepository.ActivateCurrentMovie(movie.Id);
                _moviesRepository.Save();

                MovieResultModel movieModel = new MovieResultModel
                {
                    Movie = new MovieDomainModel
                    {
                        Id = movie.Id,
                        Current = activatedMovie.Current,
                        HasOscar = movie.HasOscar,
                        Rating = movie.Rating ?? 0,
                        Title = movie.Title,
                        Year = movie.Year
                    },
                    IsSuccessful = true,
                    ErrorMessage = null
                    
                };

                return movieModel;
            }

            else
            {
                var projections = await _projectionRepository.GetAll();

                var futureProjections = projections.Where(p => p.MovieId == id && p.DateTime > DateTime.Now);

                if (futureProjections.Count() > 0)
                {
                    return new MovieResultModel
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.MOVIE_DEACTIVATION_ERROR,
                    };
                }

                var deactivatedMovie = await _moviesRepository.DeactivateCurrentMovie(movie.Id);
                _moviesRepository.Save();

                MovieResultModel movieResultModel = new MovieResultModel
                {
                    Movie = new MovieDomainModel
                    {
                        Id = deactivatedMovie.Id,
                        Current = deactivatedMovie.Current,
                        Rating = deactivatedMovie.Rating ?? 0,
                        HasOscar = deactivatedMovie.HasOscar,
                        Title = deactivatedMovie.Title,
                        Year = deactivatedMovie.Year
                    },
                    IsSuccessful = true
                };

                return movieResultModel;

            }
        }
    }
}
