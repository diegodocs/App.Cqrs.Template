using Autofac;

namespace App.Cqrs.Core.Query
{
    public class QueryDispatcher : IQueryDispatcher
    {
        private readonly IContainer container;

        public QueryDispatcher(IContainer container)
        {
            this.container = container;
        }

        public TResult Dispatch<TParameter, TResult>(TParameter query)
            where TParameter : IQuery
            where TResult : IQueryResult
        {
            var handler = container.Resolve<IQueryHandler<TParameter, TResult>>();
            return handler.Retrieve(query);
        }
    }
}