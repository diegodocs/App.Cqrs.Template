using App.Cqrs.Template.Core.Domain;
using System;

namespace App.Cqrs.Template.Core.Repository
{
    public interface IRepositoryPersistenceService<TEntity> : IDisposable where TEntity : IAggregateRoot
    {
        bool Insert(TEntity instance);
        bool Delete(int id);
        bool Update(TEntity instance);
    }
}