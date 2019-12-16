using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Domain.Models;

namespace WinterWorkShop.Cinema.Domain.Interfaces
{
    public interface IMovieService
    {
        /// <summary>
        /// Get all movies by current parameter
        /// </summary>
        /// <param name="isCurrent"></param>
        /// <returns></returns>
        IEnumerable<MovieDomainModel> GetAllMovies(bool? isCurrent);

        /// <summary>
        /// Get movie by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        MovieDomainModel GetMovieById(Guid id);

        /// <summary>
        /// Adds new movie to DB
        /// </summary>
        /// <param name="newMovie"></param>
        /// <returns></returns>
        MovieDomainModel AddMovie(MovieDomainModel newMovie);
    }
}
