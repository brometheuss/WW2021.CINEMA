using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace WinterWorkShop.Cinema.Repositories
{
    public interface IGenericRepository<T> where T : class
    {
        EntityEntry<T> Delete(object id);

        Task<IEnumerable<T>> GetAll();

        T GetById(object id);

        IEnumerable<T> Get(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, string includeProperties = "");

        EntityEntry<T> Insert(T obj);

        void Save();

        void Update(T obj);
    }
}