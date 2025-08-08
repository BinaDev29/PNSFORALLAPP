// File Path: API/Controllers/NotificationController.cs
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Notification.Commands;
using Application.CQRS.Notification.Queries;
using Application.DTO.Notification;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotifications()
        {
            var query = new GetNotificationsListQuery();
            var notifications = await mediator.Send(query);
            return Ok(notifications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotification(Guid id)
        {
            var query = new GetNotificationDetailQuery { Id = id };
            var notification = await mediator.Send(query);
            return Ok(notification);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotification([FromBody] CreateNotificationDto dto)
        {
            var command = new CreateNotificationCommand { CreateNotificationDto = dto };
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetNotification), new { id = response.Id }, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotification([FromBody] UpdateNotificationDto dto)
        {
            var command = new UpdateNotificationCommand { UpdateNotificationDto = dto };
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotification(Guid id)
        {
            var command = new DeleteNotificationCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}