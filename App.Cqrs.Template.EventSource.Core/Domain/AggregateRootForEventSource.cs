using App.Cqrs.Core.Event;
using App.Cqrs.Template.EventSource.Core.Extension;
using System;
using System.Collections.Generic;

namespace App.Cqrs.Template.EventSource.Core.Domain
{
    public abstract class AggregateRootForEventSource : IAggregateRootForEventSource
    {
        private readonly List<IEvent> _changes = new List<IEvent>();
        
        public Guid Id { get; protected set; }
        public int Version { get; protected set; }

        public IEnumerable<IEvent> GetUncommittedChanges()
        {
            return _changes;
        }

        public void MarkChangesAsCommitted()
        {
            _changes.Clear();
        }

        public void LoadsFromHistory(IEnumerable<IEvent> history)
        {
            foreach (var e in history) ApplyChange(e, false);
        }

        protected void ApplyChange(IEvent @event)
        {
            ApplyChange(@event, true);
        }

        // push atomic aggregate changes to local history for further processing (EventStore.SaveEvents)
        private void ApplyChange(IEvent @event, bool isNew)
        {
            this.AsDynamic().Apply(@event);
            if (isNew) _changes.Add(@event);
        }
    }
}