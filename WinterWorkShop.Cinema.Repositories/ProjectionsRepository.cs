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
    public interface IProjectionsRepository : IRepository<Projection> 
    {
        IEnumerable<Projection> GetBySalaId(int salaId);
    }

    public class ProjectionsRepository : IProjectionsRepository
    {
        private CinemaContext _cinemaContext;

        public ProjectionsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public EntityEntry<Projection> Delete(object id)
        {
            Projection existing = _cinemaContext.Projections.Find(id);
            return _cinemaContext.Projections.Remove(existing);
        }

        public async Task<IEnumerable<Projection>> GetAll()
        {
            var data = await _cinemaContext.Projections.ToListAsync();

            if (data != null)
            {
                return data;
            }
            else
            {
                return null;
            }
        }

        public async Task<Projection> GetByIdAsync(object id)
        {
            return await _cinemaContext.Projections.FindAsync(id);
        }

        public IEnumerable<Projection> GetBySalaId(int salaId)
        {
            var projectionsData = _cinemaContext.Projections.Where(x => x.SalaId == salaId);

            return projectionsData;
        }

        public EntityEntry<Projection> Insert(Projection obj)
        {
            var data = _cinemaContext.Projections.Add(obj);

            if (data == null)
            {
                return null;
            }

            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public EntityEntry<Projection> Update(Projection obj)
        {
            var updatedEntry = _cinemaContext.Projections.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
