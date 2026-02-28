using ecobooksi.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface IShoppingCartRepository : IGenericRepository<ShoppingCart>
    {
        public void Update(ShoppingCart product);
    }
}
