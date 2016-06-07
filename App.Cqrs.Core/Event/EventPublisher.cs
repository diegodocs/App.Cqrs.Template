using System.Collections.Generic;

namespace App.Cqrs.Core.Event
{
    public class EventPublisher<TEvent> : IEventPublisher<TEvent> where TEvent : IEvent
    {
        private readonly IEnumerable<IEventHandler<TEvent>> eventHandlerList;

        public EventPublisher(IEnumerable<IEventHandler<TEvent>> eventHandlerList)
        {
            this.eventHandlerList = eventHandlerList;
        }

        public void Publish(TEvent @event)
        {
            foreach(var handler in eventHandlerList)
                handler.Handle(@event);            
        }
    }    
}