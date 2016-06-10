using App.Cqrs.Core.Event;
using App.Cqrs.Template.Application.ReadModel;
using App.Cqrs.Template.Core.Repository;
using App.Template.Domain.Event;

namespace App.Cqrs.Template.Application.EventHandler
{

    public class EmployeeCreatedEventHandler : IEventHandler<EmployeeCreated>, IEventHandler<IEvent>
    {
        private readonly IRepository<EmployeeReadModel> employeeRepository;

        public EmployeeCreatedEventHandler(IRepository<EmployeeReadModel> employeeRepository)
        {
            this.employeeRepository = employeeRepository;
        }

        public void Handle(IEvent @event)
        {
            Handle((EmployeeCreated)@event);
        }

        public void Handle(EmployeeCreated @event)
        {
            employeeRepository.Insert(new EmployeeReadModel
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