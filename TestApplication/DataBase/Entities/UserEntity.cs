namespace TestApplication.DataBase.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<CrashEntity> Crashes { get; set; } = [];

    }
}
