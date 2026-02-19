using ecobooksi.Models.Models;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Category> Categories { get; }
        public IProductRepository Products { get; }
        int Complete();
    }
}
