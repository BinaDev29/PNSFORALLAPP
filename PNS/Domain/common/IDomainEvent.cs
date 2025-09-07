// File Path: Domain/Common/IDomainEvent.cs
using MediatR;
using System;

namespace Domain.Common
{
    public interface IDomainEvent : INotification
    {
        Guid Id { get; }
        DateTime OccurredOn { get; }
    }
}