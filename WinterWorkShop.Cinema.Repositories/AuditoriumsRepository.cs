using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IAuditoriumsRepository : IRepository<Auditorium> { }
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private CinemaContext _cinemaContext;

        public AuditoriumsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Auditorium Delete(object id)
        {
            Auditorium existing = _cinemaContext.Auditoriums.Find(id);
            var result = _cinemaContext.Auditoriums.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Auditorium>> GetAll()
        {
            var data = await _cinemaContext.Auditoriums.ToListAsync();

            if (data == null)
            {
                return null;
            }

            return data;
        }

        public async Task<Auditorium> GetByIdAsync(object id)
        {
            return await _cinemaContext.Auditoriums.FindAsync(id);
        }

        public Auditorium Insert(Auditorium obj)
        {
            return _cinemaContext.Auditoriums.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Auditorium Update(Auditorium obj)
        {
            var updatedEntry = _cinemaContext.Auditoriums.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
    }
}
