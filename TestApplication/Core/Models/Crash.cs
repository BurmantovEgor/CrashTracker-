using System.Runtime.InteropServices;
using CSharpFunctionalExtensions;
using TestApplication.DTO_s;

namespace TestApplication.Core.Models
{
    public class Crash
    {
        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public Guid StatusId { get; private set; }
        public List<Operation> Operations { get; private set; } = new();

        private Crash(Guid id, string name, string description, Guid statusId)
        {
            Id = id;
            Name = name;
            Description = description;
            StatusId = statusId;
        }

        public static Result<Crash> Create(CrashDTOCreate dto)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
                return Result.Failure<Crash>("Название не может быть пустым");

            return Result.Success(new Crash(Guid.NewGuid(), dto.Name, dto.Description, dto.Status));
        }

    }
}
