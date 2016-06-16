using App.Cqrs.Core.Event;
using System;

namespace App.Template.Domain.Event
{
    public class EmployeeUserAccountCreated : IEvent
    {
        public EmployeeUserAccountCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public int Version { get; set; }
        public Guid Id { get; protected set; }
        public string Name { get; protected set; }
    }
}