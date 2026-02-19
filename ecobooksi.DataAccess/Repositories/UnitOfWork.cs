using ecobooksi.DataAccess.Context;
using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;

namespace ecobooksi.DataAccess.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IGenericRepository<Category> Categories { get; private set; }

        public IProductRepository Products { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            Categories = new GenericRepository<Category>(context);
            Products = new ProductRepository(context);
            _context = context;
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
