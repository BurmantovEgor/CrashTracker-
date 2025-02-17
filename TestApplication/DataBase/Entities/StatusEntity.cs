namespace TestApplication.DataBase.Entities
{
    public class StatusEntity
    {
        public Guid Id { get; set; }
        public required string Name { get; set; } = string.Empty;

    }
}
