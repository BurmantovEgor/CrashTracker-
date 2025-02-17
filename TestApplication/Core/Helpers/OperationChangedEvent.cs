using MediatR;

namespace CrashTracker.Core.Helpers
{
    public class OperationChangedEvent : INotification
    {
        public Guid CrashId { get; }
        public OperationChangedEvent(Guid crashId) => CrashId = crashId;
    }

}
