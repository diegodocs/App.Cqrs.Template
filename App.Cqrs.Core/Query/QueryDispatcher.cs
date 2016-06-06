using System;
using Ninject;

namespace App.Cqrs.Core.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IKernel _kernel;

        public QueryDispatcher(IKernel kernel)
        {
            if (kernel == null)
            {
                throw new ArgumentNullException("kernel");
            }
            _kernel = kernel;
        }

        public TResult Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery
            where TResult : IQueryResult
        {
            var handler = _kernel.Get<IQueryHandler<TParameter, TResult>>();
            return handler.Retrieve(query);
        }

    }
}