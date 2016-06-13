using App.Cqrs.Core.Bus;
using App.Cqrs.Core.Command;
using App.Cqrs.Core.Event;
using Autofac;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace App.Cqrs.Template.Infrastructure.Bus
{
    public class FakeBus : IBus
    {
        private readonly IComponentContext context;
        private readonly IEnumerable<Lazy<IEventHandler<IEvent>, IHandlerMetadata>> eventHandlers;

        public FakeBus(IComponentContext context, IEnumerable<Lazy<IEventHandler<IEvent>, IHandlerMetadata>> eventHandlers)
        {
            this.context = context;
            this.eventHandlers = eventHandlers;
        }

        public void Dispatch<TCommand>(TCommand command) where TCommand : ICommand
        {
            var handler = context.Resolve<ICommandHandler<TCommand>>();
            handler.Handle(command);
        }

        public void Publish<TEvent>(TEvent @event) where TEvent : IEvent
        {
            if (@event == null)
            {
                throw new ArgumentNullException(nameof(@event));
            }
                        
            var eventHandlers = context.ResolveNamed<IEnumerable<IEventHandler<IEvent>>>(@event.GetType().Name);
            
            foreach (var handler in eventHandlers)
            {
                handler.Handle(@event);
            }
        }

        private IEnumerable<IEventHandler<TEvent>> GetHandlers<TEvent>(TEvent @event) where TEvent : IEvent
        {
            var eventType = typeof(IEventHandler<>).MakeGenericType(@event.GetType());
            var eventHandlers = context
                    .Resolve<IEnumerable<IEventHandler<TEvent>>>()
                    .Where(t => t.GetType() == eventType);

            return eventHandlers;
        }
    }
}