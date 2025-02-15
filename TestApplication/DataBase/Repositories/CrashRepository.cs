using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TestApplication.Core.Interfaces.Crash;
using TestApplication.DataBase;
using TestApplication.DataBase.Configurations;
using TestApplication.DataBase.Entities;
using TestApplication.DTO_s;

namespace TestApplication.DataBase.Repositories
{
    public class CrashRepository : ICrashRepository
    {
        private readonly CrashTrackerDbContext _context;

        public CrashRepository(CrashTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result<CrashEntity>> Insert(CrashEntity entity)
        {
            var statusExists = await _context.Status.AnyAsync(s => s.Id == entity.CrashStatusId);
            if (!statusExists)
            {
                return Result.Failure<CrashEntity>("Статус не существует");
            }

            await _context.Crash.AddAsync(entity);
            await _context.SaveChangesAsync();
            return Result.Success(entity);
        }

        public async Task<Result<List<CrashEntity>>> SelectAll()
        {
            var crashes = await _context.Crash
                .Include(c => c.Operations)
                .Include(u => u.CreatedBy)
                .AsNoTracking()
                .ToListAsync();
            return Result.Success(crashes);
        }

        public async Task<Result<CrashEntity>> SelectById(Guid id)
        {

            var crash = await _context.Crash.Include(c => c.Operations).FirstOrDefaultAsync(c => c.Id == id);
            return crash != null ? Result.Success(crash) : Result.Failure<CrashEntity>("Запись не найдена");
        }

        public async Task<Result> Delete(Guid id)
        {
            var entity = await _context.Crash.FindAsync(id);
            if (entity == null) return Result.Failure("Авария не найдена");

            _context.Crash.Remove(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> Update(CrashEntity entity)
        {
            var statusExists = await _context.Status.AnyAsync(s => s.Id == entity.CrashStatusId);
            if (!statusExists)
            {
                return Result.Failure("Статус не существует");
            }
            _context.Crash.Update(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> UpdateProgress(Guid crashId, double progress)
        {
            var entity = await _context.Crash.FindAsync(crashId);
            entity.Progress = progress;
            var result = _context.SaveChanges();
            if (result == 0) return Result.Failure($"Не удалось обновить прогресс для операции {crashId}");
            return Result.Success();
        }
    }
}
