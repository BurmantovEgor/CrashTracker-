using TestApplication.DataBase.Entities;

namespace CrashTracker.Application.DTO_s
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;

    }
}
