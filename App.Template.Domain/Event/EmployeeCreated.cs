using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Template.Domain.Event
{
    public class EmployeeCreated
    {
        public EmployeeCreated(string name, string job, int level, decimal salary)
        {
            this.Name = name;
            this.CurrentJob = job;
            this.CurrentLevel = level;
            this.CurrentSalary = salary;
         
        }

        public string Name { get; protected set; }
        public string CurrentJob { get; protected set; }
        public int CurrentLevel { get; protected set; }
        public decimal CurrentSalary { get; protected set; }
    }
}
