using App.Cqrs.Template.Core.Domain;
using App.Template.Domain.Event;
using System;
using System.Collections.Generic;

namespace App.Template.Domain.Model
{
    public class Employee : AggregateRoot
    {
        public Employee(string name, string job, int level, decimal salary)
        {
            var id = Guid.NewGuid();
            Apply(new EmployeeCreated(id, name, job, level, salary));
        }

        public string Name { get; protected set; }
        public string CurrentJob { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public decimal CurrentSalary { get; protected set; }
        public IEnumerable<JobHistory> JobHistoryList { get; protected set; }

        public class JobHistory
        {
            public string Job { get; set; }
            public decimal Salary { get; set; }
            public int Level { get; set; }
        }

        protected void Apply(EmployeeCreated @event)
        {
            Id = Id;
            Name = @event.Name;
            CurrentJob = @event.CurrentJob;
            CurrentLevel = @event.CurrentLevel;
            CurrentSalary = @event.CurrentSalary;
            JobHistoryList = new List<JobHistory>();

            OnApplied(@event);
        }

        protected void Apply(EmployeeUserAccountCreated @event)
        {
            Id = Id;
            Name = @event.Name;

            OnApplied(@event);
        }

        public void UpgradeLevel()
        {
            Apply(new EmployeeLevelUpgraded(CurrentLevel, CurrentSalary));
        }

        protected void Apply(EmployeeLevelUpgraded @event)
        {
            CurrentLevel = @event.Level;
            CurrentSalary = @event.Salary;
            OnApplied(@event);
        }

        public void ChangeJob(string job, int level, decimal salary)
        {
            Apply(new EmployeeJobChanged(job, level, salary));
        }

        protected void Apply(EmployeeJobChanged @event)
        {
            CurrentJob = @event.Job;
            CurrentLevel = @event.Level;
            CurrentSalary = @event.Salary;
            ((List<JobHistory>)JobHistoryList).Add(new JobHistory()
            {
                Job = @event.Job,
                Salary = @event.Salary,
                Level = @event.Level
            });

            OnApplied(@event);
        }
    }
}