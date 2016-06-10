using App.Cqrs.Core.Query;

namespace App.Cqrs.Template.Application.Query
{
    public class EmployeeByNameQuery : IQuery
    {
        public string Name { get; set; }
    }
}