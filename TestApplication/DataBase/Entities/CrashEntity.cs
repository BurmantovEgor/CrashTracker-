namespace TestApplication.DataBase.Entities

{
    public class CrashEntity
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public Guid StatusId { get; set; }
        public List<OperationEntity> Operations { get; set; } = [];
    }
}
