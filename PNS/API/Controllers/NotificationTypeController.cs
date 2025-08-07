using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.NotificationType.Commands;
using Application.CQRS.NotificationType.Queries;
using Application.DTO.NotificationType;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationTypeController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotificationTypes()
        {
            var query = new GetNotificationTypesListQuery();
            var notificationTypes = await mediator.Send(query);
            return Ok(notificationTypes);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationType(Guid id)
        {
            var query = new GetNotificationTypeDetailQuery { Id = id };
            var notificationType = await mediator.Send(query);
            return Ok(notificationType);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotificationType([FromBody] CreateNotificationTypeDto dto)
        {
            var command = new CreateNotificationTypeCommand { CreateNotificationTypeDto = dto };
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNotificationType([FromBody] UpdateNotificationTypeDto dto)
        {
            var command = new UpdateNotificationTypeCommand { UpdateNotificationTypeDto = dto };
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotificationType(Guid id)
        {
            var command = new DeleteNotificationTypeCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}