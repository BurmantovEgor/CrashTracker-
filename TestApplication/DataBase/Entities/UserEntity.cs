namespace TestApplication.DataBase.Entities
{
    public class UserEntity
    {
        public Guid Id { get; set; }
        public required string UserName { get; set; } = string.Empty;
        public required string UserEmail { get; set; } = string.Empty;
        public required string PasswordHash { get; set; } = string.Empty;
    }
}
