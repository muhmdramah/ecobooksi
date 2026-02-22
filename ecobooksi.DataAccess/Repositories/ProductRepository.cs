using ecobooksi.DataAccess.Context;
using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models;

namespace ecobooksi.DataAccess.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public void Update(Product product)
        {
            var currentProduct = _context.Products.FirstOrDefault(p => p.ProductId == product.ProductId);

            if(currentProduct != null)
            {
                currentProduct.Title = product.Title;
                currentProduct.Description = product.Description;
                currentProduct.ISBN = product.ISBN;
                currentProduct.Author = product.Author;
                currentProduct.ListPrice = product.ListPrice;
                currentProduct.Price = product.Price;
                currentProduct.PriceFifty = product.PriceFifty;
                currentProduct.PriceHundred = product.PriceHundred;
                currentProduct.CategoryId = product.CategoryId;

                if(product.ImageURL != null)
                {
                    currentProduct.ImageURL = product.ImageURL;
                }
            }
        }
    }
}
