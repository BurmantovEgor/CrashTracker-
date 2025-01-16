using TestApplication.DataBase.Entities;

namespace TestApplication.Interfaces.Status
{
    public interface IStatusRepository
    {
        Task<List<StatusEntity>> SelectAll();
        Task<StatusEntity> SelectById(Guid statusId);

    }
}
