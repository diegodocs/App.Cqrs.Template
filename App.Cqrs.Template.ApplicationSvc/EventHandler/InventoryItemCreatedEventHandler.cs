using App.Cqrs.Core.Event;
using App.Cqrs.Template.Application.ReadModel;
using App.Cqrs.Template.Core.Repository;
using App.Template.Domain.Event;

namespace App.Cqrs.Template.Application.EventHandler
{
    public class InventoryItemCreatedEventHandler : IEventHandler<InventoryItemCreated>, IEventHandler<IEvent>
    {
        private readonly IRepository<InventoryItemReadModel> repository;

        public InventoryItemCreatedEventHandler(IRepository<InventoryItemReadModel> repository)
        {
            this.repository = repository;
        }

        public void Handle(IEvent @event)
        {
            Handle((InventoryItemCreated)@event);
        }

        public void Handle(InventoryItemCreated @event)
        {
            repository.Insert(new InventoryItemReadModel { Id = @event.Id, Name = @event.Name, Version = @event.Version });
        }
    }

    public class InventoryItemRenamedEventHandler : IEventHandler<InventoryItemRenamed>, IEventHandler<IEvent>
    {
        private readonly IRepository<InventoryItemReadModel> repository;

        public InventoryItemRenamedEventHandler(IRepository<InventoryItemReadModel> repository)
        {
            this.repository = repository;
        }

        public void Handle(IEvent @event)
        {
            Handle((InventoryItemCreated)@event);
        }

        public void Handle(InventoryItemRenamed @event)
        {
            var inventoryItem = repository.Find(r => r.Id == @event.Id);
            inventoryItem.Name = @event.NewName;
            inventoryItem.Version = @event.Version;

            repository.Update(inventoryItem);
        }
    }
}