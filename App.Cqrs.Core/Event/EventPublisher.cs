using System.Collections.Generic;

namespace App.Cqrs.Core.Event
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IEnumerable<IEventHandler<IEvent>> eventHandlerList;

        public EventPublisher(IEnumerable<IEventHandler<IEvent>> eventHandlerList)
        {
            this.eventHandlerList = eventHandlerList;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            foreach (var handler in eventHandlerList)
                handler.Handle(@event);
        }
    }
}