using App.Cqrs.Template.Core.Domain;

namespace App.Cqrs.Template.Core.Repository
{
    public interface IRepository<TEntity> :
        IRepositoryPersistenceService<TEntity>,
        IRepositoryQueryService<TEntity> where TEntity : IAggregateRoot
    {
    }
}