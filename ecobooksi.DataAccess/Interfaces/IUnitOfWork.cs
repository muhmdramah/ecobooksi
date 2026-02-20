using ecobooksi.Models.Models;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Category> Category { get; }
        public IProductRepository Product { get; }
        int Complete();
    }
}
