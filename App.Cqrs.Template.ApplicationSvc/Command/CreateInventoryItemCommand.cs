using App.Cqrs.Core.Command;
using System;

namespace App.Cqrs.Template.Application.Command
{
    public class CreateInventoryItemCommand : ICommand
    {
        public Guid InventoryItemId
        {
            get;
            protected set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public CreateInventoryItemCommand(Guid inventoryItemId, string name)
        {
            InventoryItemId = inventoryItemId;
            Name = name;
        }
    }
}