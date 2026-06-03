using IRepository;
using MyCRM.DataBase;
using Microsoft.EntityFrameworkCore;
using MyCRM.Models;
using MyCRM.Common;
using Microsoft.Extensions.Options;

namespace Repository
{
    public class PermissionRepository : BaseRepository<TPermission>, IPermissionRepository
    {
        private ApplicationDbcontext _dbcontext;
        private readonly IOptions<SystemItem> _options;
        private readonly DbContextOptions<ApplicationDbcontext> _dbOptions;

        public PermissionRepository(ApplicationDbcontext dbcontext, IOptions<SystemItem> options) :base(dbcontext)
        {
            _dbcontext = dbcontext;
            _options = options;
            // _dbOptions = dbOptions;, DbContextOptions<ApplicationDbcontext> dbOptions
        }

     
    }
}
