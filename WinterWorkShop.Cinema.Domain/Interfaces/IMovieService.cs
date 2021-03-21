using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Domain.Models;
using WinterWorkShop.Cinema.Domain.Queries;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IMovieService
    {
        /// <summary>
        /// Get all movies by current parameter
        /// </summary>
        /// <param name="isCurrent"></param>
        /// <returns></returns>
        IEnumerable<MovieDomainModel> GetAllMovies(bool? isCurrent, MovieQuery query);

        /// <summary>
        /// Get a movie by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MovieDomainModel> GetMovieByIdAsync(Guid id);

        /// <summary>
        /// Adds new movie to DB
        /// </summary>
        /// <param name="newMovie"></param>
        /// <returns></returns>
        Task<MovieDomainModel> AddMovie(MovieDomainModel newMovie);

        /// <summary>
        /// Update a movie to DB
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MovieDomainModel> UpdateMovie(MovieDomainModel updateMovie);

        /// <summary>
        /// Delete a movie by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MovieDomainModel> DeleteMovie(Guid id);
        Task<MovieResultModel> ActivateMovie(Guid id);
        Task<MovieResultModel> DeactivateMovie(Guid id);
        Task<IEnumerable<MovieDomainModel>> GetTopMovies();
        Task<IEnumerable<MovieDomainModel>> GetTopByYear(int year);
        Task<IEnumerable<MovieDomainModel>> GetAllMoviesNonCurrentIncluded();
        Task<MovieResultModel> Activate_DeactivateMovie(Guid id);
        Task<IEnumerable<MovieProjectionDomainModel>> GetCurrentMoviesAndProjections();
        Task<IEnumerable<MovieDomainModel>> GetMoviesByAuditoriumId(int id);
        Task<IEnumerable<MovieDomainModel>> GetAllMoviesFilterWithNonCurrent(bool? isCurrent, MovieQuery query);
    }
}
