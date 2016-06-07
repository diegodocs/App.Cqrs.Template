using App.Cqrs.Template.Core.Domain;
using System;

namespace App.Cqrs.Template.Core.Repository
{
    public interface IRepository<TEntity> :
        IRepositoryPersistenceService<TEntity>,
        IRepositoryQueryService<TEntity>, IDisposable where TEntity : IEntityBase
    {
    }
}