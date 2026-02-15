using System.Linq.Expressions;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<ICollection<T>> GetAllAsync();
        public Task<T> GetAsync(Expression<Func<T, bool>> filter);
        public Task Create(T entity);
        public Task Delete(T entity);
        public Task DeleteRange(IEnumerable<T> entities);

        //public Task UpdateAsync(T entity);
        //public Task SaveAsync();
    }
}
