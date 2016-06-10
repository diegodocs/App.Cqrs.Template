using App.Cqrs.Template.EventSource.Core.Domain;
using App.Template.Domain.Event;
using System;

namespace App.Template.Domain.Model
{
    public class InventoryItem : AggregateRootForEventSource
    {        
        public  string Name { get; protected set; }
        public  bool Activated { get; protected set; }
        public InventoryItem()
        {
        }       

        public InventoryItem(Guid id, string name)
        {
            ApplyChange(new InventoryItemCreated(id, name));
        }

        public void ChangeName(string newName)
        {
            if (string.IsNullOrEmpty(newName)) throw new ArgumentException("newName");
            ApplyChange(new InventoryItemRenamed(this.Id, newName));
        }
    }
}