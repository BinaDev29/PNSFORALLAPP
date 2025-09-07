// File Path: API/Controllers/NotificationHistoryController.cs
using Application.CQRS.NotificationHistory.Commands;
using Application.CQRS.NotificationHistory.Queries;
using Application.DTO.NotificationHistory;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/NotificationHistory
        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationHistoryDto>), 200)]
        public async Task<ActionResult<List<NotificationHistoryDto>>> Get()
        {
            var query = new GetNotificationHistoriesListQuery();
            var histories = await _mediator.Send(query);
            return Ok(histories);
        }

        // GET: api/NotificationHistory/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotificationHistoryDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NotificationHistoryDto>> Get(Guid id)
        {
            var query = new GetNotificationHistoryDetailQuery { Id = id };
            var history = await _mediator.Send(query);
            return Ok(history);
        }

        // POST: api/NotificationHistory
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateNotificationHistoryDto createNotificationHistoryDto)
        {
            var command = new CreateNotificationHistoryCommand { CreateNotificationHistoryDto = createNotificationHistoryDto };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // Note: Update and Delete are not implemented for NotificationHistory model
        // as per the provided CQRS code, which only includes Create and Query.
    }
}