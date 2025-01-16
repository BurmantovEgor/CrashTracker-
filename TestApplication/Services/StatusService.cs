using TestApplication.DataBase.Entities;
using TestApplication.Interfaces.Status;

namespace TestApplication.Services
{
    public class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        public StatusService(IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;
        }

        public async Task<List<StatusEntity>> GetAll()
        {
            return await _statusRepository.SelectAll();

        }

        public async Task<StatusEntity> GetById(Guid id)
        {
            return await _statusRepository.SelectById(id);
        }
    }
}
