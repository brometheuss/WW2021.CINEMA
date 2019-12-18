using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IAuditoriumRepository : IRepository<Auditorium> { }
    public class AuditoriumRepository
    {
        private CinemaContext _cinemaContext;

        public AuditoriumRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public EntityEntry<Auditorium> Delete(object id)
        {
            Auditorium existing = _cinemaContext.Auditoriums.Find(id);
            return _cinemaContext.Auditoriums.Remove(existing);
        }

        public async Task<IEnumerable<Auditorium>> GetAll()
        {
            return await _cinemaContext.Auditoriums.ToListAsync();
        }

        public async Task<Auditorium> GetByIdAsync(object id)
        {
            return await _cinemaContext.Auditoriums.FindAsync(id);
        }

        public EntityEntry<Auditorium> Insert(Auditorium obj)
        {
            return _cinemaContext.Auditoriums.Add(obj);
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public EntityEntry<Auditorium> Update(Auditorium obj)
        {
            var updatedEntry = _cinemaContext.Auditoriums.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
