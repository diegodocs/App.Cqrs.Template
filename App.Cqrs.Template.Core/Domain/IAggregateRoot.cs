using App.Cqrs.Core.Event;
using System;
using System.Collections.Generic;

namespace App.Cqrs.Template.Core.Domain
{
    public interface IAggregateRoot : IEntityBase
    {
        int Version
        {
            get;
        }
        IEnumerable<IEvent> AppliedEvents { get; }
    }
}