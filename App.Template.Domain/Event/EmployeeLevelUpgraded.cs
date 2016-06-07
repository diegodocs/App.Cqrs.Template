using App.Cqrs.Core.Event;

namespace App.Template.Domain.Event
{
    public class EmployeeLevelUpgraded 
    {
        public EmployeeLevelUpgraded( int level, decimal salary)
        {            
            this.Level = level ++;
            this.Salary = salary * 1.1m;

        }
        
        public int Level { get; protected set; }
        public decimal Salary { get; protected set; }
    }
}
