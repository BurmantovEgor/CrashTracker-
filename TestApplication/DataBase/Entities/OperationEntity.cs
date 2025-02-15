namespace TestApplication.DataBase.Entities
{
    public class OperationEntity
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public Guid CrashId { get; set; }
        public bool IsCompleted { get; set; }

    }
}
