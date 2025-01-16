using TestApplication.DataBase.Entities;

namespace TestApplication.DTO_s
{
    public class CrashDTO
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string StatusName { get; set; }
        public List<OperationDTO> Operations { get; set; } = [];

    }
}
