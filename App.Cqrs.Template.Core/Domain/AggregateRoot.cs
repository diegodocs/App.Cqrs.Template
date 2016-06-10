using System;
using System.Collections.Generic;

namespace App.Cqrs.Template.Core.Domain
{
    public class AggregateRoot : IAggregateRoot
    {
        private readonly List<object> appliedEvents = new List<object>();

        public Guid Id { get; protected set; }

        public int Version { get; protected set; }

        public IEnumerable<object> AppliedEvents
        {
            get { return appliedEvents; }
        }

        protected void OnApplied(object @event)
        {
            appliedEvents.Add(@event);
            Version++;
        }
    }
}