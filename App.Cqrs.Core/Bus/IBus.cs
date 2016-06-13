using App.Cqrs.Core.Command;
using App.Cqrs.Core.Event;

namespace App.Cqrs.Core.Bus
{
    public interface IBus : ICommandDispatcher, IEventPublisher
    {
    }

    public interface IHandlerMetadata
    {
        string TypeName { get; set; }
    }
}