using App.Cqrs.Core.Event;
using System;
using System.Collections.Generic;

namespace App.Cqrs.Template.EventSource.Core.Repository
{
    public interface IEventStore
    {
        void SaveEvents(Guid aggregateId, IEnumerable<IEvent> events, int expectedVersion);

        List<IEvent> GetEventsForAggregate(Guid aggregateId);
    }
}