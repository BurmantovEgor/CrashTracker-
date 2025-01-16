using Microsoft.EntityFrameworkCore;
using TestApplication.Core.Interfaces.Status;
using TestApplication.DataBase.Configurations;
using TestApplication.DataBase.Entities;

namespace TestApplication.DataBase.Repositories
{
    public class StatusRepository : IStatusRepository
    {
        private readonly CrashTrackerDbContext _context;

        public StatusRepository(CrashTrackerDbContext context)
        {
            _context = context;

        }

        public async Task<List<StatusEntity>> SelectAll()
        {
            return await _context.Status.AsNoTracking().ToListAsync();
        }

        public async Task<StatusEntity> SelectById(Guid statusId)
        {
            return await _context.Status.AsNoTracking().FirstOrDefaultAsync(x=>x.Id==statusId);
        }
    }
}
