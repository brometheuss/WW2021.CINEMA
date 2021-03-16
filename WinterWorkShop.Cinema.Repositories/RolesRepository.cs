using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;
using WinterWorkShop.Cinema.Data.Entities;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IRolesRepository : IRepository<Role> { }
    public class RolesRepository : IRolesRepository
    {
        
        private CinemaContext _cinemaContext;

        public RolesRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public Role Delete(object id)
        {
            Role existing = _cinemaContext.Roles.Find(id);
            var result = _cinemaContext.Roles.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<Role>> GetAll()
        {
            var data = await _cinemaContext.Roles
                .Include(user => user.Users )
                .ToListAsync();

            return data;
        }

        public async Task<Role> GetByIdAsync(object id)
        {
            var data = await _cinemaContext.Roles
                .Include(user => user.Users)
                .FirstOrDefaultAsync(role => role.Id == (int)id);

            return data;
        }

        public Role Insert(Role obj)
        {
            return _cinemaContext.Roles.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public Role Update(Role obj)
        {
            var updatedEntry = _cinemaContext.Roles.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
    }
}
