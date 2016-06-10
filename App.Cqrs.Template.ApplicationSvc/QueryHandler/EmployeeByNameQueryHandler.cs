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
        private readonly IRepository<EmployeeReadModel> repositoryDTOEmployee;

        public EmployeeByNameQueryHandler(IRepository<EmployeeReadModel> repositoryDTOEmployee)
        {
            this.repositoryDTOEmployee = repositoryDTOEmployee;
        }

        public EmployeeByNameQueryResult Retrieve(EmployeeByNameQuery query)
        {
            var result = new EmployeeByNameQueryResult();
            result.EmployeeList = repositoryDTOEmployee.All().Where(x => x.Name == query.Name).ToList();

            return result;
        }
    }
}