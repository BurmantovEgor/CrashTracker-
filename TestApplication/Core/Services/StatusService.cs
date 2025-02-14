using Microsoft.EntityFrameworkCore;
using TestApplication.Core.Interfaces.Status;
using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Services
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


        public async Task<List<StatusEntity>> GetStatusesById(List<Guid> statusIds)
        {
            return await _statusRepository.GetStatusesById(statusIds);
        }

    }
}
