using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ICitiesRepository : IRepository<City> { }
    public class CitiesRepository : ICitiesRepository
    {
        private CinemaContext _cinemaContext;

        public CitiesRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public City Delete(object id)
        {
            City existing = _cinemaContext.Cities.Find(id);
            var result = _cinemaContext.Cities.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<City>> GetAll()
        {
            var data = await _cinemaContext.Cities.Include(c => c.Cinemas).ToListAsync();

            return data;
        }

        public async Task<City> GetByIdAsync(object id)
        {
              
            var data = await _cinemaContext.Cities.Include(c => c.Cinemas).FirstOrDefaultAsync(x => x.Id == (int)id);

            return data;
        }

        public City Insert(City obj)
        {
            return _cinemaContext.Cities.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public City Update(City obj)
        {
            var updatedEntry = _cinemaContext.Cities.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
    }
}
