using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IUsersRepository : IRepository<User> { }
    public class UsersRepository : IUsersRepository
    {
        private CinemaContext _cinemaContext;

        public UsersRepository(CinemaContext cinemaContext)
        {
            _cinemaContext = cinemaContext;
        }

        public User Delete(object id)
        {
            User existing = _cinemaContext.Users.Find(id);
            var result = _cinemaContext.Users.Remove(existing);

            return result.Entity;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var data = await _cinemaContext.Users.ToListAsync();

            if (data == null)
            {
                return null;
            }

            return data;
        }

        public async Task<User> GetByIdAsync(object id)
        {
            return await _cinemaContext.Users.FindAsync(id);
        }

        public User Insert(User obj)
        {
            return _cinemaContext.Users.Add(obj).Entity;
        }

        public void Save()
        {
            _cinemaContext.SaveChanges();
        }

        public User Update(User obj)
        {
            var updatedEntry = _cinemaContext.Users.Attach(obj);
            _cinemaContext.Entry(obj).State = EntityState.Modified;

            return updatedEntry.Entity;
        }
    }
}
