using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Interfaces.Status
{
    public interface IStatusService
    {
        Task<List<StatusEntity>> GetAll();
        Task<StatusEntity> GetById(Guid id);
        Task<List<StatusEntity>> GetStatusesById(List<Guid> statusIds);

    }
}
