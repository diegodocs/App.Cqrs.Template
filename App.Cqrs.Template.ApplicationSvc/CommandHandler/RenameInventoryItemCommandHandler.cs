using App.Cqrs.Core.Command;
using App.Cqrs.Template.Application.Command;
using App.Cqrs.Template.EventSource.Core.Repository;
using App.Template.Domain.Model;

namespace App.Cqrs.Template.Application.CommandHandler
{
    public class RenameInventoryItemCommandHandler : ICommandHandler<RenameInventoryItemCommand>
    {
        private readonly IRepositoryForEventSource<InventoryItem> inventoryRepository;

        public RenameInventoryItemCommandHandler(IRepositoryForEventSource<InventoryItem> inventoryRepository)
        {
            this.inventoryRepository = inventoryRepository;
        }

        public void Handle(RenameInventoryItemCommand command)
        {
            var item = new InventoryItem(command.InventoryItemId, command.NewName);
            inventoryRepository.Save(item, 1);
        }
    }
}