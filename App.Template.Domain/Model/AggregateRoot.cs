
using App.Cqrs.Template.Core.Domain;
using System.Collections.Generic;

namespace App.Template.Domain.Model
{
    public class AggregateRoot : IAggregateRoot
    {
        private readonly List<object> appliedEvents = new List<object>();

        public System.Guid Id { get; protected set; }

        public int Version { get; protected set; }   
        
        protected void Initiate()
        {
            this.Id = System.Guid.NewGuid();
            this.Version = 1;
        }            

        public IEnumerable<object> AppliedEvents
        {
            get { return appliedEvents; }
        }

        protected void OnApplied(object @event)
        {
            appliedEvents.Add(@event);
            Version++;
        }
    }
}
