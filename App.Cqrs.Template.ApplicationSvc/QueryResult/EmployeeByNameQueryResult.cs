using App.Cqrs.Core.Query;
using App.Cqrs.Template.Application.ReadModel;
using System.Collections.Generic;

namespace App.Cqrs.Template.Application.QueryResult
{
    public class EmployeeByNameQueryResult : IQueryResult
    {
        public IEnumerable<EmployeeReadModel> EmployeeList { get; set; }
    }
}