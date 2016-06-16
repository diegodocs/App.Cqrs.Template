using App.Cqrs.Core.Command;

namespace App.Cqrs.Template.Application.Command
{
    public class EmployeeCreateCommand : ICommand
    {
        public EmployeeCreateCommand(string name, string job, int level, decimal salary)
        {
            Name = name;
            Job = job;
            Level = level;
            Salary = salary;
        }

        public string Name { get; protected set; }
        public string Job { get; protected set; }
        public int Level { get; protected set; }
        public decimal Salary { get; protected set; }
    }
}