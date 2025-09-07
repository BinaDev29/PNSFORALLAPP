// File Path: Infrastructure/Services/DomainEventService.cs
using Application.Common.Interfaces;
using Domain.Common;
using MediatR;
using Microsoft.Extensions.Logging;
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
                _logger.LogInformation("Publishing domain event {EventType} with ID {EventId}", 
                    domainEvent.GetType().Name, domainEvent.Id);

                await _mediator.Publish(domainEvent);

                _logger.LogInformation("Successfully published domain event {EventType} with ID {EventId}", 
                    domainEvent.GetType().Name, domainEvent.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error publishing domain event {EventType} with ID {EventId}", 
                    domainEvent.GetType().Name, domainEvent.Id);
                throw;
            }
        }
    }
}