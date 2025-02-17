namespace TestApplication.DataBase.Entities
{
    public class OperationEntity
    {
        public Guid Id { get; set; }
        public required string Description { get; set; }
        public required Guid CrashId { get; set; }
        public required bool IsCompleted { get; set; }

    }
}
