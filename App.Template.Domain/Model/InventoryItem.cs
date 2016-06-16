using App.Cqrs.Template.EventSource.Core.Domain;
using App.Template.Domain.Event;
using System;

namespace App.Template.Domain.Model
{
    public class InventoryItem : AggregateRootForEventSource
    {
        public string Name { get; protected set; }
        public bool Activated { get; protected set; }

        public InventoryItem()
        {
        }

        public InventoryItem(Guid id, string name)
        {
            ApplyChange(new InventoryItemCreated(id, name));
        }

        protected void ApplyChange(InventoryItemCreated @event)
        {
            Id = @event.Id;
            Name = @event.Name;
            Version = @event.Version;
            OnApplied(@event);
        }

        public void ChangeName(string newName)
        {
            ApplyChange(new InventoryItemRenamed(Id, newName));
        }

        protected void ApplyChange(InventoryItemRenamed @event)
        {
            Name = @event.NewName;
            OnApplied(@event);
        }
    }
}