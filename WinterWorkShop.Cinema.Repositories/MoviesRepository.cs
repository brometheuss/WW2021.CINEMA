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
    }

    public class MoviesRepository : IMoviesRepository
    {
        private CinemaContext _cinemaContext;

        public MoviesRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public EntityEntry<Movie> Delete(object id)
        {
            Movie existing = _cinemaContext.Movies.Find(id);

            if (existing == null)
            {
                return null;
            }

            return _cinemaContext.Movies.Remove(existing);
        }

        public async Task<IEnumerable<Movie>> GetAll()
        {
            return await _cinemaContext.Movies.ToListAsync();
        }

        public async Task<Movie> GetByIdAsync(object id)
        {
            Movie existing = await _cinemaContext.Movies.FindAsync(id);

            if (existing != null)
            {
                return existing;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Movie> GetCurrentMovies()
        {
            var data = _cinemaContext.Movies
                .AsParallel()
                .Where(x => x.Current);

            if (data != null)
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        public EntityEntry<Movie> Insert(Movie obj)
        {
            var existing = _cinemaContext.Movies.Add(obj);

            if (existing != null)
            {
                return existing;
            }
            else
            {
                return null; 
            }
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public EntityEntry<Movie> Update(Movie obj)
        {
            var updatedEntry = _cinemaContext.Movies.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
