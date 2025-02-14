using TestApplication.DataBase.Entities;

namespace TestApplication.Core.Interfaces.Status
{
    public interface IStatusRepository
    {
        Task<List<StatusEntity>> SelectAll();
        Task<StatusEntity> SelectById(Guid statusId);
        Task<List<StatusEntity>> GetStatusesById(List<Guid> statusIds);
    }
}
