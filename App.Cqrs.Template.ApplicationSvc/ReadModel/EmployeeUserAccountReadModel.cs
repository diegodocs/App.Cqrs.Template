
using System;
using App.Cqrs.Template.Core.Domain;

namespace App.Cqrs.Template.ApplicationSvc.ReadModel
{
    public class EmployeeUserAccountReadModel : IEntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserAccount { get; set; }              
    }
}
