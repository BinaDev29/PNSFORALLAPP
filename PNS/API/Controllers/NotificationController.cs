// File Path: API/Controllers/NotificationController.cs
using Application.CQRS.Notification.Commands;
using Application.CQRS.Notification.Queries;
using Application.DTO.Notification;
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
    public class NotificationController : ControllerBase
    {
        private readonly IMediator _mediator;

        public NotificationController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Notification
        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationDto>), 200)]
        public async Task<ActionResult<List<NotificationDto>>> Get()
        {
            var query = new GetNotificationsListQuery();
            var notifications = await _mediator.Send(query);
            return Ok(notifications);
        }

        // GET: api/Notification/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(NotificationDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<NotificationDto>> Get(Guid id)
        {
            var query = new GetNotificationDetailQuery { Id = id };
            var notification = await _mediator.Send(query);
            return Ok(notification);
        }

        // POST: api/Notification
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateNotificationDto createNotificationDto)
        {
            var command = new CreateNotificationCommand { CreateNotificationDto = createNotificationDto };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT: api/Notification
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdateNotificationDto updateNotificationDto)
        {
            var command = new UpdateNotificationCommand { UpdateNotificationDto = updateNotificationDto };
            await _mediator.Send(command);
            return NoContent();
        }

        // PUT: api/Notification/5/seen
        // ይህ አፕሊኬሽን ላይ notification ሲታይ የሚያገለግል endpoint ነው።
        [HttpPut("{id}/seen")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> MarkAsSeen(Guid id)
        {
            var command = new MarkNotificationAsSeenCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        // GET: api/Notification/5/track
        // ይህ ኢሜይል ሲከፈት ብቻ SeenTimeን ለመመዝገብ የሚያገለግል endpoint ነው።
        [HttpGet("{id}/track")]
        public async Task<ActionResult> TrackEmailOpen(Guid id)
        {
            var command = new MarkNotificationAsSeenCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/Notification/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteNotificationCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}