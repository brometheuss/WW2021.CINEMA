using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IActorsRepository : IRepository<Actor> { }
    public class ActorsRepository : IActorsRepository
    {

        private CinemaContext _cinemaContext;

        public ActorsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Actor Delete(object id)
        {
            Actor existing = _cinemaContext.Actors.Find(id);
            var result = _cinemaContext.Actors.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Actor>> GetAll()
        {
            var data = await _cinemaContext.Actors.ToListAsync();

            return data;
        }

        public async Task<Actor> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Actors.FindAsync(id);

            return data;
        }

        public Actor Insert(Actor obj)
        {
            return _cinemaContext.Actors.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Actor Update(Actor obj)
        {
            var updatedEntry = _cinemaContext.Actors.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
    }
}
