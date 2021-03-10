﻿using Microsoft.EntityFrameworkCore;
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
        IEnumerable<Projection> GetByAuditoriumId(int salaId);        
    }

    public class ProjectionsRepository : IProjectionsRepository
    {
        private CinemaContext _cinemaContext;

        public ProjectionsRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Projection Delete(object id)
        {
            Projection existing = _cinemaContext.Projections.Find(id);
            var result = _cinemaContext.Projections.Remove(existing).Entity;

            return result;
        }

        public async Task<IEnumerable<Projection>> GetAll()
        {
            var data = await _cinemaContext.Projections
                .Include(x => x.Auditorium)
                .ThenInclude(c => c.Cinema)
                .Include(m => m.Movie)
                .ToListAsync();
            
            return data;           
        }

        public async Task<Projection> GetByIdAsync(object id)
        {
            return await _cinemaContext.Projections
                .Include(a => a.Auditorium)
                .Include(m => m.Movie)
                .FirstOrDefaultAsync(x => x.Id.ToString() == id.ToString());
        }

        public IEnumerable<Projection> GetByAuditoriumId(int auditoriumId)
        {
            var projectionsData = _cinemaContext.Projections.Where(x => x.AuditoriumId == auditoriumId);

            return projectionsData;
        }

        public Projection Insert(Projection obj)
        {
            var data = _cinemaContext.Projections.Add(obj).Entity;

            return data;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Projection Update(Projection obj)
        {
            var updatedEntry = _cinemaContext.Projections.Attach(obj).Entity;
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry;
        }
    }
}
