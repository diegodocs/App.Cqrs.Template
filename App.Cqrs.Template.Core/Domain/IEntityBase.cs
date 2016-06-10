using System;

namespace App.Cqrs.Template.Core.Domain
{
    public interface IEntityBase
    {
        Guid Id { get; }
    }
}