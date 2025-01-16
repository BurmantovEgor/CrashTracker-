
namespace TestApplication.DTO_s
{
    public class OperationDTO
    {
        public Guid Id { get; set; }
        public string Description { get; set; }

        public OperationDTO(Guid id, string description)
        {
            Id = id;
            Description = description;
        }
    }
}
