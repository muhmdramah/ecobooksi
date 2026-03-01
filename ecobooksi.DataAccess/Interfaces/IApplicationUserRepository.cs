using ecobooksi.Models.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobooksi.DataAccess.Interfaces
{
    public interface IApplicationUserRepository : IGenericRepository<ApplicationUser>
    {
    }
}
