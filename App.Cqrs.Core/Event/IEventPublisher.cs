namespace App.Cqrs.Core.Event
{
    public interface IEventPublisher
    {
        void Publish<TEvent>(TEvent @event) where TEvent : IEvent;
    }
}