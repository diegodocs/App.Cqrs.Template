

using App.Template.Domain.Event;
using System;
using System.Collections.Generic;

namespace App.Template.Domain.Model
{
    public class Employee : AggregateRoot
    {
        public Employee(string name, string job, int level, decimal salary)
        {
            Apply(new EmployeeCreated(name, job, level, salary));
        }

        public string Name { get; protected set; }
        public string CurrentJob { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public decimal CurrentSalary { get; protected set; }
        public IEnumerable<JobHistory> JobHistoryList { get; protected set; }
        public class JobHistory
        {
            public string Job { get;  set; }
            public decimal Salary { get;  set; }
            public int Level { get;  set; }
        }

        protected void Apply(EmployeeCreated @event)
        {
            this.Initiate();
            this.Name = @event.Name;
            this.CurrentJob = @event.CurrentJob;
            this.CurrentLevel = @event.CurrentLevel;
            this.CurrentSalary = @event.CurrentSalary;
            this.JobHistoryList = new List<JobHistory>();
            
            OnApplied(@event);
        }

        public void UpgradeLevel()
        {
            Apply(new EmployeeLevelUpgraded(this.CurrentLevel, this.CurrentSalary));            
        }

        protected void Apply(EmployeeLevelUpgraded @event)
        {
            this.CurrentLevel = @event.Level;
            this.CurrentSalary = @event.Salary;
            OnApplied(@event);
        }

        public void ChangeJob(string job, int level, decimal salary)
        {
            Apply(new EmployeeJobChanged(job,level,salary));
        }

        protected void Apply(EmployeeJobChanged @event)
        {
            this.CurrentJob = @event.Job;
            this.CurrentLevel = @event.Level;
            this.CurrentSalary = @event.Salary;
            ((List<JobHistory>)this.JobHistoryList).Add(new JobHistory() {
                Job = @event.Job,
                Salary = @event.Salary,
                Level = @event.Level});
            
            OnApplied(@event);
        }
    }
}
