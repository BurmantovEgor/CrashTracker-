namespace TestApplication.DataBase.Entities

{
    public class CrashEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public required Guid CrashStatusId { get; set; }
        public List<OperationEntity> Operations { get; set; } = [];
        public required Guid CreatedById { get; set; }
        public UserEntity CreatedBy { get; set; }
        public required double Progress { get; set; } = 0;
    }
}
