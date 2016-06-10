﻿using App.Cqrs.Template.EventSource.Core.Domain;
using System;

namespace App.Cqrs.Template.EventSource.Core.Repository
{
    public class RepositoryForEventSource<T> : IRepositoryForEventSource<T> where T : AggregateRootForEventSource, new() //shortcut you can do as you see fit with new()
    {
        private readonly IEventStore _storage;

        public RepositoryForEventSource(IEventStore storage)
        {
            _storage = storage;
        }

        public void Save(AggregateRootForEventSource aggregate, int expectedVersion)
        {
            _storage.SaveEvents(aggregate.Id, aggregate.GetUncommittedChanges(), expectedVersion);
        }

        public T GetById(Guid id)
        {
            var obj = new T();
            var e = _storage.GetEventsForAggregate(id);
            obj.LoadsFromHistory(e);
            return obj;
        }
    }
}