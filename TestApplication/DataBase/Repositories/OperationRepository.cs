using CSharpFunctionalExtensions;
using Microsoft.EntityFrameworkCore;
using TestApplication.Core.Interfaces.Operations;
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
            try
            {
                await _context.Operation.AddAsync(operationEntity);
                await _context.SaveChangesAsync();
                return Result.Success(operationEntity);
            }
            catch (DbUpdateException)
            {
                return Result.Failure<OperationEntity>("Ошибка при вставке данных в базу");
            }
            catch (InvalidOperationException)
            {
                return Result.Failure<OperationEntity>("Ошибка работы с базой данных");
            }
            catch (Exception)
            {
                return Result.Failure<OperationEntity>("Внутренняя ошибка сервера");
            }
        }

        public async Task<Result> Update(OperationEntity operationEntity)
        {
            try
            {
                var entity = await _context.Operation.FindAsync(operationEntity.Id);
                if (entity == null)
                    return Result.Failure("Операция не найдена");

                entity.Description = operationEntity.Description;
                entity.IsCompleted = operationEntity.IsCompleted;

                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateException)
            {
                return Result.Failure("Ошибка при обновлении данных в базе");
            }
            catch (InvalidOperationException)
            {
                return Result.Failure("Ошибка работы с базой данных");
            }
            catch (Exception)
            {
                return Result.Failure("Внутренняя ошибка сервера");
            }
        }

        public async Task<Result> Delete(Guid id)
        {
            try
            {
                var entity = await _context.Operation.FindAsync(id);
                if (entity == null)
                    return Result.Failure("Операция не найдена");

                _context.Operation.Remove(entity);
                await _context.SaveChangesAsync();
                return Result.Success();
            }
            catch (DbUpdateException)
            {
                return Result.Failure("Ошибка при удалении данных в базе");
            }
            catch (InvalidOperationException)
            {
                return Result.Failure("Ошибка работы с базой данных");
            }
            catch (Exception)
            {
                return Result.Failure("Внутренняя ошибка сервера");
            }
        }

        public async Task<Result<List<OperationEntity>>> SelectByCrashId(Guid crashId)
        {
            try
            {
                var operations = await _context.Operation
                    .Where(x => x.CrashId == crashId)
                    .ToListAsync();

                return operations.Count > 0
                    ? Result.Success(operations)
                    : Result.Failure<List<OperationEntity>>("Операции не найдены");
            }
            catch (Exception)
            {
                return Result.Failure<List<OperationEntity>>("Ошибка при получении данных из базы");
            }
        }

        public async Task<Result<OperationEntity>> SelectById(Guid id)
        {
            try
            {
                var operation = await _context.Operation
                                .FirstOrDefaultAsync(x => x.Id == id);

                return operation != null
                    ? Result.Success(operation)
                    : Result.Failure<OperationEntity>("Операция не найдена");
            }
            catch (Exception)
            {
                return Result.Failure<OperationEntity>("Ошибка при получении операции из базы");
            }
        }
    }
}
