// File Path: Infrastructure/Services/DomainEventService.cs
using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class DomainEventService : IDomainEventService
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DomainEventService> _logger;

        public DomainEventService(IMediator mediator, ILogger<DomainEventService> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        public async Task PublishAsync(IDomainEvent domainEvent)
        {
            try
            {
                // Better logging for clarity
                _logger.LogInformation("Publishing domain event: {EventType} for AggregateId: {AggregateId}",
                    domainEvent.GetType().Name, ((AggregateRoot)domainEvent).Id);

                await _mediator.Publish(domainEvent);

                _logger.LogInformation("Successfully published domain event: {EventType}",
                    domainEvent.GetType().Name);
            }
            catch (Exception ex)
            {
                // Log the exception with a clear message and stack trace
                _logger.LogError(ex, "Failed to publish domain event {EventType}. Exception: {Message}",
                    domainEvent.GetType().Name, ex.Message);
                throw;
            }
        }
    }
}