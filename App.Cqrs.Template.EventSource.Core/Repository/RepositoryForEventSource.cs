using App.Cqrs.Template.EventSource.Core.Domain;
using System;

namespace App.Cqrs.Template.EventSource.Core.Repository
{
    public class RepositoryForEventSource<T> : IRepositoryForEventSource<T> where T : AggregateRootForEventSource, new() //shortcut you can do as you see fit with new()
    {
        private readonly IEventStore storage;

        public RepositoryForEventSource(IEventStore storage)
        {
            this.storage = storage;
        }

        public void Save(AggregateRootForEventSource aggregate, int expectedVersion)
        {
            storage.SaveEvents(aggregate.Id, aggregate.AppliedEvents, expectedVersion);
        }

        public T GetById(Guid id)
        {
            var instance = new T();
            var events = storage.GetEventsForAggregate(id);
            instance.LoadsFromHistory(events);
            instance.MarkChangesAsCommitted();
            return instance;
        }
    }
}