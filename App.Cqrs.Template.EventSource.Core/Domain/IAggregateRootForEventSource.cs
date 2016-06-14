using App.Cqrs.Core.Event;
using System;
using System.Collections.Generic;

namespace App.Cqrs.Template.EventSource.Core.Domain
{
    public interface IAggregateRootForEventSource
    {
        Guid Id { get; }
        int Version { get; }
        IEnumerable<IEvent> AppliedEvents { get; }

        void LoadsFromHistory(IEnumerable<IEvent> history);

        void MarkChangesAsCommitted();
    }
}