// File Path: API/Controllers/NotificationTypeController.cs
using Application.CQRS.NotificationType.Commands;
using Application.CQRS.NotificationType.Queries;
using Application.DTO.NotificationType;
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
    public class NotificationTypeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/NotificationType
        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationTypeDto>), 200)]
        public async Task<ActionResult<List<NotificationTypeDto>>> Get()
        {
            var query = new GetNotificationTypesListQuery();
            var types = await _mediator.Send(query);
            return Ok(types);
        }

        // GET: api/NotificationType/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotificationTypeDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NotificationTypeDto>> Get(Guid id)
        {
            var query = new GetNotificationTypeDetailQuery { Id = id };
            var type = await _mediator.Send(query);
            return Ok(type);
        }

        // POST: api/NotificationType
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateNotificationTypeDto createNotificationTypeDto)
        {
            var command = new CreateNotificationTypeCommand { CreateNotificationTypeDto = createNotificationTypeDto };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT: api/NotificationType
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdateNotificationTypeDto updateNotificationTypeDto)
        {
            var command = new UpdateNotificationTypeCommand { UpdateNotificationTypeDto = updateNotificationTypeDto };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/NotificationType/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteNotificationTypeCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}