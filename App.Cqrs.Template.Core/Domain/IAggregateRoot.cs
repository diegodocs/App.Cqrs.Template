using System;
using System.Collections.Generic;

namespace App.Cqrs.Template.Core.Domain
{    
    public interface IAggregateRoot<out TId>
    {        
        TId Id { get; }

        int Version { get; }

        IEnumerable<object> AppliedEvents { get; }
    }

    public interface IAggregateRoot : IAggregateRoot<Guid>
    {
    }
}