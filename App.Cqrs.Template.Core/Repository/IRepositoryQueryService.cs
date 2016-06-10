using App.Cqrs.Template.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace App.Cqrs.Template.Core.Repository
{
    public interface IRepositoryQueryService<TEntity> where TEntity : IEntityBase
    {
        TEntity Find(Expression<Func<TEntity, bool>> expression);

        IEnumerable<TEntity> All();

        IEnumerable<TEntity> FindList(Expression<Func<TEntity, bool>> expression);
    }
}