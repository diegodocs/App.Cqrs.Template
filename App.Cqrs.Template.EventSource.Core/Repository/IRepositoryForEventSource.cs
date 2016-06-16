using App.Cqrs.Template.EventSource.Core.Domain;
using System;

namespace App.Cqrs.Template.EventSource.Core.Repository
{
    public interface IRepositoryForEventSource<out TAggregate>
        where TAggregate : AggregateRootForEventSource, new()
    {
        void Save(AggregateRootForEventSource aggregate, int expectedVersion);

        TAggregate GetById(Guid id);
    }
}