namespace TestApplication.DataBase.Entities

{
    public class CrashEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid CrashStatusId { get; set; }
        public List<OperationEntity> Operations { get; set; } = [];
        public Guid CreatedById { get; set; }
        public UserEntity CreatedBy { get; set; }
        public double Progress { get; set; } = 0;
    }
}
