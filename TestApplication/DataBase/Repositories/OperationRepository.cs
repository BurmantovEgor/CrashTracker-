using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TestApplication.Core.Interfaces.Operations;
using TestApplication.DataBase;
using TestApplication.DataBase.Configurations;
using TestApplication.DataBase.Entities;

namespace TestApplication.DataBase.Repositories
{
    public class OperationRepository : IOperationRepository
    {
        private readonly CrashTrackerDbContext _context;

        public OperationRepository(CrashTrackerDbContext context)
        {
            _context = context;
        }

        public async Task<Result<OperationEntity>> Insert(OperationEntity operationEntity)
        {
            await _context.Operation.AddAsync(operationEntity);
            await _context.SaveChangesAsync();
            return Result.Success(operationEntity);
        }

        public async Task<Result> Update(OperationEntity operationEntity)
        {
            var entity = await _context.Operation.FindAsync(operationEntity.Id);
            if (entity == null) return Result.Failure("Операция не найдена");

            entity.Description = operationEntity.Description;
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result> Delete(Guid id)
        {
            var entity = await _context.Operation.FindAsync(id);
            if (entity == null) return Result.Failure("Операция не найдена");

            _context.Operation.Remove(entity);
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        public async Task<Result<List<OperationEntity>>> SelectByCrashId(Guid crashId)
        {
            var operations = await _context.Operation
                .Where(x => x.CrashId == crashId)
                .ToListAsync();

            return Result.Success(operations);
        }

        public async Task<Result<OperationEntity>> SelectById(Guid crashId)
        {
            var operation = await _context.Operation
                            .FirstOrDefaultAsync(x => x.Id == crashId);
            if (operation == null) return Result.Failure<OperationEntity>("Операция не найдена");
            return Result.Success(operation);
        }
    }
}
