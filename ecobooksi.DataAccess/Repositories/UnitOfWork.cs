    using ecobooksi.DataAccess.Context;
    using ecobooksi.DataAccess.Interfaces;
    using ecobooksi.Models.Models;

    namespace ecobooksi.DataAccess.Repositories
    {
        public class UnitOfWork : IUnitOfWork
        {
            private readonly ApplicationDbContext _context;
            public IGenericRepository<Category> Category { get; private set; }

            public IProductRepository Product { get; private set; }
            public ICompanyRepository Company { get; private set; }
            public IShoppingCartRepository ShoppingCart { get; private set; }
            public IApplicationUserRepository ApplicationUser { get; private set; }

            public UnitOfWork(ApplicationDbContext context)
            {
                Category = new GenericRepository<Category>(context);
                Product = new ProductRepository(context);
                Company = new CompanyRepository(context);
                ShoppingCart = new ShoppingCartRepository(context);
                ApplicationUser = new ApplicationUserRepository(context);
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
