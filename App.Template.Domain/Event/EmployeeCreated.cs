using App.Cqrs.Core.Event;
using System;

namespace App.Template.Domain.Event
{
    public class EmployeeCreated : IEvent
    {
        public EmployeeCreated(Guid id, string name, string job, int level, decimal salary)
        {
            this.Id = id;
            this.Name = name;
            this.CurrentJob = job;
            this.CurrentLevel = level;
            this.CurrentSalary = salary;
        }

        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
        public string CurrentJob { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public decimal CurrentSalary { get; protected set; }
    }
}