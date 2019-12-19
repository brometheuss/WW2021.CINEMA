using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface ICinemasRepository : IRepository<Data.Cinema> { }
    public class CinemasRepository : ICinemasRepository
    {
        private CinemaContext _cinemaContext;

        public CinemasRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public EntityEntry<Data.Cinema> Delete(object id)
        {
            Data.Cinema existing = _cinemaContext.Cinemas.Find(id);
            return _cinemaContext.Cinemas.Remove(existing);
        }

        public async Task<IEnumerable<Data.Cinema>> GetAll()
        {
            return await _cinemaContext.Cinemas.ToListAsync();
        }

        public async Task<Data.Cinema> GetByIdAsync(object id)
        {
            return await _cinemaContext.Cinemas.FindAsync(id);
        }

        public EntityEntry<Data.Cinema> Insert(Data.Cinema obj)
        {
            return _cinemaContext.Cinemas.Add(obj);
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public EntityEntry<Data.Cinema> Update(Data.Cinema obj)
        {
            var updatedEntry = _cinemaContext.Cinemas.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
