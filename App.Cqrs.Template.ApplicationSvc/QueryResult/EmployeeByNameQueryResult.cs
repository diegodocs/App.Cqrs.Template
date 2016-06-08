using App.Cqrs.Core.Query;
using App.Cqrs.Template.ApplicationSvc.ReadModel;
using System.Collections.Generic;

namespace App.Cqrs.Template.ApplicationSvc.QueryResult
{
    public class EmployeeByNameQueryResult : IQueryResult
    {        
        public IEnumerable<EmployeeReadModel> EmployeeList { get; set; }
    }
}