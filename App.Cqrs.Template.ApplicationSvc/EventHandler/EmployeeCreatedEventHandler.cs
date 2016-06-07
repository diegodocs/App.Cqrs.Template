using App.Cqrs.Core.Event;
using App.Cqrs.Template.ApplicationSvc.DTO;
using App.Cqrs.Template.Core.Repository;
using App.Template.Domain.Event;

namespace App.Cqrs.Template.ApplicationSvc.CommandHandler
{
    public class EmployeeCreatedEventHandler : IEventHandler<EmployeeCreated>
    {
        private readonly IRepository<DTOEmployee> employeeRepository;
        public EmployeeCreatedEventHandler(IRepository<DTOEmployee> employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public void Handle(EmployeeCreated @event)
        {
            employeeRepository.Insert(new DTOEmployee
            {
                Id = @event.Id,
                Name = @event.Name,
                CurrentJob = @event.CurrentJob,
                CurrentLevel = @event.CurrentLevel,
                CurrentSalary = @event.CurrentSalary
            });
        }
    }
}
