using System.Linq.Expressions;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public ICollection<T> GetAll();
        public Task<T> GetAsync(Expression<Func<T, bool>> filter);
        public Task CreateAsync(T entity);
        public Task DeleteAsync(T entity);
        public Task DeleteRangeAsync(IEnumerable<T> entities);

        public void Update(T entity);
    }
}
