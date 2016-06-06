using System;
using Ninject;

namespace App.Cqrs.Core.Event
{
    public class EventPublisher : IEventPublisher
    {
        private readonly IKernel _kernel;

        public EventPublisher(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            _kernel = kernel;
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var handler = _kernel.Get<IEventHandler<TEvent>>();
            handler.Handle(@event);
        }
    }
}