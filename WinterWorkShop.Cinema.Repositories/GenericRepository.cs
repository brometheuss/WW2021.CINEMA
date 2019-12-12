using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using WinterWorkShop.Cinema.Data;

namespace WinterWorkShop.Cinema.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private CinemaContext _context = null;
        private DbSet<T> _table = null;

        public GenericRepository(CinemaContext cinemaContext)
        {
            _context = cinemaContext;
            _table = _context.Set<T>();
        }

        public async virtual Task<IEnumerable<T>> GetAll()
        {
            return await _table.ToListAsync();
        }

        public virtual IEnumerable<T> Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "")
        {
            IQueryable<T> query = _table;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeProperties != null)
            {
                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public T GetById(object id)
        {
            return _table.Find(id);
        }

        public EntityEntry<T> Insert(T obj)
        {
            return _table.Add(obj);
        }

        public void Update(T obj)
        {
            _table.Attach(obj);
            _context.Entry(obj).State = EntityState.Modified;
        }

        public EntityEntry<T> Delete(object id)
        {
            T existing = _table.Find(id);
            return _table.Remove(existing);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
