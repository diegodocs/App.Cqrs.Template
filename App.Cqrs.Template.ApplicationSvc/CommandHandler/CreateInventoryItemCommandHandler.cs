using App.Cqrs.Core.Command;
using App.Cqrs.Template.Application.Command;
using App.Cqrs.Template.EventSource.Core.Repository;
using App.Template.Domain.Model;

namespace App.Cqrs.Template.Application.CommandHandler
{
    public class CreateInventoryItemCommandHandler : ICommandHandler<CreateInventoryItemCommand>
    {
        private readonly IRepositoryForEventSource<InventoryItem> inventoryRepository;
        public CreateInventoryItemCommandHandler(IRepositoryForEventSource<InventoryItem> inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public void Handle(CreateInventoryItemCommand command)
        {
            var item = new InventoryItem(command.InventoryItemId, command.Name);
            inventoryRepository.Save(item, -1);
        }
    }
}