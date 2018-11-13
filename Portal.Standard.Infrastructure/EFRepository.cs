using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Portal.core;
using Portal.core.Infrastructure;

namespace Portal.Standard.Infrastructure
{
    public class EFRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly PortalDbContext _dbContext;

        public EFRepository(PortalDbContext marketingContext)
        {
            _dbContext = marketingContext;
        }
        public T GetById(object id)
        {
            return _dbContext.Set<T>().Find(id);
        }

        public void Insert(T entity)
        {
            _dbContext.Set<T>().Add(entity);
            _dbContext.SaveChanges();
        }

        public void Insert(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().AddRange(entities);
            _dbContext.SaveChanges();
        }

        public void Update(T entity)
        {
            _dbContext.SaveChanges();
        }

        public void Update(IEnumerable<T> entities)
        {
            _dbContext.SaveChanges();
        }

        public void Delete(T entity)
        {
            _dbContext.Set<T>().Remove(entity);
            _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<T> entities)
        {
            _dbContext.Set<T>().RemoveRange(entities);
            _dbContext.SaveChanges();
        }

        public IQueryable<T> Table => _dbContext.Set<T>();

        public IQueryable<T> TableNoTracking => _dbContext.Set<T>().AsNoTracking();
    }
}
