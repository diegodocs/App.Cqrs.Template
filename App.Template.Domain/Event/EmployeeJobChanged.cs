using App.Cqrs.Core.Event;

namespace App.Template.Domain.Event
{
    public class EmployeeJobChanged : IEvent
    {
        public EmployeeJobChanged(string job, int level, decimal salary)
        {
            this.Job = job;
            this.Level = level;
            this.Salary = salary;
        }

        public int Version { get; set; }
        public string Job { get; protected set; }
        public int Level { get; protected set; }
        public decimal Salary { get; protected set; }
    }
}