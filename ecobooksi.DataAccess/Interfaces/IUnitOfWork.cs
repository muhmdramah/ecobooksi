using ecobooksi.Models.Models;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        public IGenericRepository<Category> Category { get; }
        public IProductRepository Product { get; }
        public ICompanyRepository Company { get; }
        public IShoppingCartRepository ShoppingCart { get; }
        public IApplicationUserRepository ApplicationUser { get; }

        public IOrderHeaderRepository OrderHeader { get; }
        public IOrderDetailRepository OrderDetail { get; }

        int Complete();
    }
}
