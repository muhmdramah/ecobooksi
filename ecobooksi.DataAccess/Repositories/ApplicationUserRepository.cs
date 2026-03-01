using ecobooksi.DataAccess.Context;
using ecobooksi.DataAccess.Interfaces;
using ecobooksi.Models.Models.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobooksi.DataAccess.Repositories
{
    public class ApplicationUserRepository : 
        GenericRepository<ApplicationUser>, IApplicationUserRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationUserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
