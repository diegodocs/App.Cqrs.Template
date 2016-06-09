using System.Collections.Generic;
using System;
using App.Cqrs.Core.Bus;
using App.Cqrs.Core.Command;
using App.Cqrs.Core.Event;
using Autofac;

namespace App.Cqrs.Template.Infrastructure.Bus
{
    public class FakeBus : IBus
    {
        private readonly IComponentContext context;

        public FakeBus(IComponentContext context)
        {
            this.context = context;
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

            var eventHandlers = context.Resolve<IEnumerable<IEventHandler<TEvent>>>();
            foreach (var handler in  eventHandlers)
            {
                handler.Handle(@event);
            }
        }
    }
}