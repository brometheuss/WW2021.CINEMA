using System;
using System.Collections.Generic;
using System.Text;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IMoviesRepository : IGenericRepository<Movie> { }

    public class MoviesRepository : GenericRepository<Movie>, IMoviesRepository
    {
        public MoviesRepository(CinemaContext cinemaContext) : base(cinemaContext)
        {
        }
    }
}
