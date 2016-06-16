using App.Cqrs.Template.Core.Domain;
using App.Cqrs.Template.Core.Repository;
using System;
using System.Collections.Generic;

namespace App.Cqrs.Template.Test.Unit.Infrastructure
{
    public class PersistenceServiceInMemory<TEntity> : IRepositoryPersistenceService<TEntity> where TEntity : IEntityBase
    {
        public readonly List<TEntity> Repository = new List<TEntity>();

        public bool Update(TEntity instancia)
        {
            Delete(instancia.Id);
            return Insert(instancia);
        }

        public bool Delete(Guid id)
        {
            Repository.RemoveAll(x => x.Id == id);
            return true;
        }

        public bool Insert(TEntity instancia)
        {
            Repository.Add(instancia);

            return true;
        }

        public void Dispose()
        {
        }
    }
}