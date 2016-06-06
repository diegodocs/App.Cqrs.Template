using App.Cqrs.Core.Command;
using App.Cqrs.Core.Event;
using App.Cqrs.Template.ApplicationSvc.Command;
using App.Cqrs.Template.Core.Repository;
using App.Template.Domain.Model;

namespace App.Cqrs.Template.ApplicationSvc.CommandHandler
{
    public class EmployeeCreateCommandHandler : ICommandHandler<EmployeeCreateCommand>
    {
        private readonly IRepository<Employee> employeeRepository;
        private readonly IEventPublisher eventPublisher;
        public EmployeeCreateCommandHandler(IRepository<Employee> employeeRepository, IEventPublisher eventPublisher)
        {            
            this.employeeRepository = employeeRepository;
            this.eventPublisher = eventPublisher;
        }

        public void Execute(EmployeeCreateCommand command)
        {
            var employee = new Employee(command.Name, command.Job, command.Level, command.Salary);

            employeeRepository.Insert(employee);

            foreach (var @event in employee.AppliedEvents)
                eventPublisher.Publish((IEvent)@event);
        }
    }
}
