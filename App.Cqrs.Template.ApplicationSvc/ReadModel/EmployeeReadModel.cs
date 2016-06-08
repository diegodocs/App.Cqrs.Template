
using System;
using App.Cqrs.Template.Core.Domain;

namespace App.Cqrs.Template.ApplicationSvc.ReadModel
{
    public class EmployeeReadModel : IEntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string CurrentJob { get; set; }
        public int CurrentLevel { get; set; }
        public decimal CurrentSalary { get; set; }        
    }
}
