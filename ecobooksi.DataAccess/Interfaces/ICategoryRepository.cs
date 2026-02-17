using ecobooksi.Models.Models;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        public void Update(Category category);
    }
}
