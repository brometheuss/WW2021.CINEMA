using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ISeatsRepository : IRepository<Seat> { }
    public class SeatsRepository : ISeatsRepository
    {
        private CinemaContext _cinemaContext;

        public SeatsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public EntityEntry<Seat> Delete(object id)
        {
            Seat existing = _cinemaContext.Seats.Find(id);
            return _cinemaContext.Seats.Remove(existing);
        }

        public async Task<IEnumerable<Seat>> GetAll()
        {
            return await _cinemaContext.Seats.ToListAsync();
        }

        public async Task<Seat> GetByIdAsync(object id)
        {
            return await _cinemaContext.Seats.FindAsync(id);
        }

        public EntityEntry<Seat> Insert(Seat obj)
        {
            return _cinemaContext.Seats.Add(obj);
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public EntityEntry<Seat> Update(Seat obj)
        {
            var updatedEntry = _cinemaContext.Seats.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
