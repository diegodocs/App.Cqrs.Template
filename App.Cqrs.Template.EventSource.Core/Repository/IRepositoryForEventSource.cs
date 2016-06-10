using App.Cqrs.Template.EventSource.Core.Domain;
using System;

namespace App.Cqrs.Template.EventSource.Core.Repository
{
    public interface IRepositoryForEventSource<T>
        where T : AggregateRootForEventSource, new()
    {
        void Save(AggregateRootForEventSource aggregate, int expectedVersion);

        T GetById(Guid id);
    }
}