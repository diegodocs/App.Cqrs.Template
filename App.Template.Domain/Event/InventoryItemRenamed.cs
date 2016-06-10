using App.Cqrs.Core.Event;
using System;

namespace App.Template.Domain.Event
{
    public class InventoryItemRenamed : IEvent
    {
        public Guid Id
        {
            get;
            protected set;
        }

        public string NewName
        {
            get;
            protected set;
        }

        public int Version
        {
            get;
            set;
        }

        public InventoryItemRenamed(Guid id, string newName)
        {
            Id = id;
            NewName = newName;
        }
    }
}