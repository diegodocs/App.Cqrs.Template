using App.Cqrs.Core.Query;
using App.Cqrs.Template.ApplicationSvc.DTO;
using App.Cqrs.Template.ApplicationSvc.Query;
using App.Cqrs.Template.ApplicationSvc.QueryResult;
using App.Cqrs.Template.Core.Repository;
using System.Linq;

namespace App.Cqrs.Template.ApplicationSvc.QueryHandler
{
    public class EmployeeByNameQueryHandler : IQueryHandler<EmployeeByNameQuery, EmployeeByNameQueryResult>
    {
        private readonly IRepository<DTOEmployee> repositoryDTOEmployee;

        public EmployeeByNameQueryHandler(IRepository<DTOEmployee> repositoryDTOEmployee)
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