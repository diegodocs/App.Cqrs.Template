using App.Cqrs.Template.Core.Domain;
using System;

namespace App.Cqrs.Template.Application.ReadModel
{
    public class EmployeeUserAccountReadModel : IEntityBase
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserAccount { get; set; }
    }
}