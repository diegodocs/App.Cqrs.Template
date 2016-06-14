using App.Cqrs.Core.Command;
using App.Cqrs.Core.Event;
using App.Cqrs.Template.Application.Command;
using App.Cqrs.Template.Core.Repository;
using App.Template.Domain.Model;

namespace App.Cqrs.Template.Application.CommandHandler
{
    public class EmployeeCreateCommandHandler : ICommandHandler<EmployeeCreateCommand>
    {
        private readonly IRepositoryPersistenceService<Employee> employeeRepository;
        private readonly IEventPublisher eventPublisher;

        public EmployeeCreateCommandHandler(IRepositoryPersistenceService<Employee> employeeRepository, IEventPublisher eventPublisher)
        {
            this.employeeRepository = employeeRepository;
            this.eventPublisher = eventPublisher;
        }

        public void Handle(EmployeeCreateCommand command)
        {
            var employee = new Employee(command.Name, command.Job, command.Level, command.Salary);
            employeeRepository.Insert(employee);
            foreach (var @event in employee.AppliedEvents)
                eventPublisher.Publish((IEvent)@event);
        }
    }
}