// File Path: Domain/Common/DomainEvent.cs
using System;

namespace Domain.Common
{
    public abstract class DomainEvent : IDomainEvent
    {
        public Guid Id { get; private set; }
        public DateTime OccurredOn { get; private set; }

        protected DomainEvent()
        {
            Id = Guid.NewGuid();
            OccurredOn = DateTime.UtcNow;
        }
    }
}