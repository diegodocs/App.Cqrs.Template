using App.Cqrs.Core.Query;
using App.Cqrs.Template.Application.Query;
using App.Cqrs.Template.Application.QueryResult;
using App.Cqrs.Template.Application.ReadModel;
using App.Cqrs.Template.Core.Repository;
using System.Linq;

namespace App.Cqrs.Template.Application.QueryHandler
{
    public class EmployeeByNameQueryHandler : IQueryHandler<EmployeeByNameQuery, EmployeeByNameQueryResult>
    {
        private readonly IRepository<EmployeeReadModel> repositoryDtoEmployee;

        public EmployeeByNameQueryHandler(IRepository<EmployeeReadModel> repositoryDtoEmployee)
        {
            this.repositoryDtoEmployee = repositoryDtoEmployee;
        }

        public EmployeeByNameQueryResult Retrieve(EmployeeByNameQuery query)
        {
            var result = new EmployeeByNameQueryResult
            {
                EmployeeList = repositoryDtoEmployee.All().Where(x => x.Name == query.Name).ToList()
            };

            return result;
        }
    }
}