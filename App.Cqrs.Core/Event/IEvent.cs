using App.Cqrs.Core.Bus;

namespace App.Cqrs.Core.Event
{
    public interface IEvent : IMessage
    {
        int Version { get; set; }
    }
}