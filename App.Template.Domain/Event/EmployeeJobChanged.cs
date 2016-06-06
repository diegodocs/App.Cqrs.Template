using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Template.Domain.Event
{
    public class EmployeeJobChanged
    {
        public EmployeeJobChanged( string job, int level, decimal salary)
        {            
            this.Job = job;
            this.Level = level;
            this.Salary = salary;
         
        }
        
        public string Job { get; protected set; }
        public int Level { get; protected set; }
        public decimal Salary { get; protected set; }
    }
}
