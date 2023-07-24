using Microsoft.EntityFrameworkCore;
using SmokeyBook.DataAccess.Data;
using SmokeyBook.DataAccess.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SmokeyBook.DataAccess.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDBContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDBContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();

            _db.Products.Include(x => x.Category).Include(x => x.CategoryId);
        }

        public void Add(T entity)
        {
            dbSet.Add(entity);
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperties = null)
        {
            IQueryable<T> query = dbSet;
            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperties))
            {
                // This part splits the includeProperties string into an array of property names using the comma (,) as the delimiter. It removes any empty entries from the resulting array.

                var _includePro = includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                //This loop iterates over the array of property names obtained in the previous step.
                foreach (var includePro in _includePro)
                {
                    query = query.Include(includePro);

                }
            }

            return query.FirstOrDefault();
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> query = dbSet;
            return query.ToList();
        }

        public void Remove(T entity)
        {
            _db.Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            _db.RemoveRange(entities);
        }
    }
}
