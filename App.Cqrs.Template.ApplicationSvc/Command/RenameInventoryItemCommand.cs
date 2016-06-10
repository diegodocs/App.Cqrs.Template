using App.Cqrs.Core.Command;
using System;

namespace App.Cqrs.Template.Application.Command
{
    public class RenameInventoryItemCommand : ICommand
    {
        public Guid InventoryItemId
        {
            get;
            protected set;
        }

        public string NewName
        {
            get;
            protected set;
        }

        public int OriginalVersion
        {
            get;
            protected set;
        }

        public RenameInventoryItemCommand(Guid inventoryItemId, string newName, int originalVersion)
        {
            InventoryItemId = inventoryItemId;
            NewName = newName;
            OriginalVersion = originalVersion;
        }
    }
}