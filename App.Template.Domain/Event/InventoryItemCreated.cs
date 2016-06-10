using App.Cqrs.Core.Event;
using System;

namespace App.Template.Domain.Event
{
    public class InventoryItemCreated : IEvent
    {
        public Guid Id
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public int Version
        {
            get;
            set;
        }

        public InventoryItemCreated(Guid id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}