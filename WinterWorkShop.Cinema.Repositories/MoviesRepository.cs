using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IMoviesRepository : IRepository<Movie> 
    {
        IEnumerable<Movie> GetCurrentMovies();
        Task<IEnumerable<Movie>> GetCurrentMoviesAsync();
        Task<Movie> DeactivateCurrentMovie(Guid obj);
        Task<Movie> ActivateCurrentMovie(Guid obj);
        Task<IEnumerable<Movie>> GetMoviesByAuditoriumId(int id);
    }

    public class MoviesRepository : IMoviesRepository
    {
        private CinemaContext _cinemaContext;

        public MoviesRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public async Task<Movie> ActivateCurrentMovie(Guid id)
        {
            var movie = await _cinemaContext.Movies.FindAsync(id);
            movie.Current = true;
            _cinemaContext.Entry(movie).State = EntityState.Modified;

            return movie;
        }

        public async Task<IEnumerable<Movie>> GetMoviesByAuditoriumId(int id)
        {
            var movies = await _cinemaContext.Movies
                .Include(projection => projection.Projections)
                .ThenInclude(auditorium => auditorium.Auditorium)
                .Where(m => m.Projections.Any(p => p.AuditoriumId == id)).ToListAsync();

            return movies;
        }

        public async Task<Movie> DeactivateCurrentMovie(Guid id)
        {
            var movie = await _cinemaContext.Movies.FindAsync(id);
            movie.Current = false;
            _cinemaContext.Entry(movie).State = EntityState.Modified;

            return movie;
        }

        public Movie Delete(object id)
        {
            Movie existing = _cinemaContext.Movies.Find(id);

            if (existing == null)
            {
                return null;
            }

            var result = _cinemaContext.Movies.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _cinemaContext.Movies
                .Include(ma => ma.MovieActors)
                .ThenInclude(a => a.Actor)
                .ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Movies.FindAsync(id);

            return data;
        }

        public IEnumerable<Movie> GetCurrentMovies()
        {
            var data = _cinemaContext.Movies
                .Include(ma => ma.MovieActors)
                .ThenInclude(a => a.Actor)
                .Where(x => x.Current);            

            return data;
        }

        public async Task<IEnumerable<Movie>> GetCurrentMoviesAsync()
        {
            var data = await _cinemaContext.Movies
                .Include(p => p.Projections)
                .ThenInclude(a => a.Auditorium)
                .Where(movie => movie.Current).ToListAsync();

            return data;
        }

        public Movie Insert(Movie obj)
        {
            var data = _cinemaContext.Movies.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Movie Update(Movie obj)
        {
            var updatedEntry = _cinemaContext.Movies.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
