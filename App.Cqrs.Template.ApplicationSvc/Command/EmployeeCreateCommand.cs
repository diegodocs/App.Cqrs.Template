using App.Cqrs.Core.Command;

namespace App.Cqrs.Template.ApplicationSvc.Command
{
    public class EmployeeCreateCommand : ICommand
    {
        public EmployeeCreateCommand(string name, string job, int level, decimal salary)
        {
            this.Name = name;
            this.Job = job;
            this.Level = level;
            this.Salary = salary;
        }

        public string Name { get; protected set; }
        public string Job { get; protected set; }
        public int Level { get; protected set; }
        public decimal Salary { get; protected set; }
    }
}