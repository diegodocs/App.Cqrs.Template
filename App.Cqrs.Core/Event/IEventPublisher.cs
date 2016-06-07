namespace App.Cqrs.Core.Event
{
    public interface IEventPublisher<in TEvent> where TEvent : IEvent
    {
        void Publish(TEvent @event);
    }
}