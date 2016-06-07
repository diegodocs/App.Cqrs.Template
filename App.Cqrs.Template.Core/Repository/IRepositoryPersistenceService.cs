using App.Cqrs.Template.Core.Domain;
using System;

namespace App.Cqrs.Template.Core.Repository
{
    public interface IRepositoryPersistenceService<TEntity> where TEntity : IEntityBase
    {
        bool Insert(TEntity instance);
        bool Delete(Guid id);
        bool Update(TEntity instance);
    }
}