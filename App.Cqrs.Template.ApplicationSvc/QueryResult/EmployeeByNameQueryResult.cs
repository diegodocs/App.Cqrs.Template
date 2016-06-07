using App.Cqrs.Core.Query;
using App.Cqrs.Template.ApplicationSvc.DTO;
using System.Collections.Generic;

namespace App.Cqrs.Template.ApplicationSvc.QueryResult
{
    public class EmployeeByNameQueryResult : IQueryResult
    {        
        public IEnumerable<DTOEmployee> EmployeeList { get; set; }
    }
}