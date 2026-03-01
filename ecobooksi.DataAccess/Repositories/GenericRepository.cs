using ecobooksi.DataAccess.Context;
using ecobooksi.DataAccess.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace ecobooksi.DataAccess.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public  ICollection<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperty = null)
        {
            IQueryable<T> query = _context.Set<T>();

            if(filter is not null)
            {
                query = query.Where(filter);
            }

            //query = query.Include(includeProperty);

            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var property in includeProperty
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }

            return  query.ToList();
        }

        public T Get(Expression<Func<T, bool>> filter, string? includeProperty = null,
             bool tracked = false)
        {
            IQueryable<T> query = _context.Set<T>();

            if (tracked == false)
            {
                query = query.AsNoTracking();
            }
            else
            {
                query = query.AsTracking();
            }

            query = query.Where(filter);

            if (!string.IsNullOrEmpty(includeProperty))
            {
                foreach (var property in includeProperty
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(property);
                }
            }
            return query.FirstOrDefault();
        }

        public async Task CreateAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(T entity)
        {
            _context.Set<T>().Remove(entity);
            _context.SaveChanges();
        }

        public async Task DeleteRangeAsync(IEnumerable<T> entities)
        {
            _context.Set<T>().RemoveRange(entities);
            _context.SaveChanges();
        }

        public void Update(T entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
        }
    }
}
