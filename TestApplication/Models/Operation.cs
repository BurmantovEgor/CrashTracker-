using CSharpFunctionalExtensions;

namespace TestApplication.Models
{
    public class Operation
    {
        public Guid Id { get;  set; }
        public string Description { get;  set; }
        public Guid CrashId { get;  set; }

        private Operation(Guid id, string description, Guid crashId)
        {
            Id = id;
            Description = description;
            CrashId = crashId;
        }

        public static Result<Operation> Create(Guid crashId, string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Result.Failure<Operation>("Описание не может быть пустым");

            return Result.Success(new Operation(Guid.NewGuid(), description, crashId));
        }
    }
}
