
namespace TestApplication.DTO_s
{
    public class OperationDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public OperationDTO(Guid id, string description, bool isCompleted)
        {
            Id = id;
            Description = description;
            IsCompleted = isCompleted;
        }
    }
}
