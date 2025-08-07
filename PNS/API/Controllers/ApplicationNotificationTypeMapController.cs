using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.CQRS.ApplicationNotificationTypeMap.Queries;
using Application.DTO.ApplicationNotificationTypeMap;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApplicationNotificationTypeMapController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetApplicationNotificationTypeMaps()
        {
            var query = new GetApplicationNotificationTypeMapsListQuery();
            var maps = await mediator.Send(query);
            return Ok(maps);
        }

        // Composite keyን ለመጠቀም Routeን አስተካክል
        [HttpGet("{clientApplicationId}/{notificationTypeId}")]
        public async Task<IActionResult> GetApplicationNotificationTypeMap(Guid clientApplicationId, Guid notificationTypeId)
        {
            var query = new GetApplicationNotificationTypeMapDetailQuery { ClientApplicationId = clientApplicationId, NotificationTypeId = notificationTypeId };
            var map = await mediator.Send(query);
            return Ok(map);
        }

        [HttpPost]
        public async Task<IActionResult> CreateApplicationNotificationTypeMap([FromBody] CreateApplicationNotificationTypeMapDto dto)
        {
            var command = new CreateApplicationNotificationTypeMapCommand { MapDto = dto };
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateApplicationNotificationTypeMap([FromBody] UpdateApplicationNotificationTypeMapDto dto)
        {
            var command = new UpdateApplicationNotificationTypeMapCommand { UpdateApplicationNotificationTypeMapDto = dto };
            await mediator.Send(command);
            return NoContent();
        }

        // Composite keyን ለመጠቀም Routeን አስተካክል
        [HttpDelete("{clientApplicationId}/{notificationTypeId}")]
        public async Task<IActionResult> DeleteApplicationNotificationTypeMap(Guid clientApplicationId, Guid notificationTypeId)
        {
            var command = new DeleteApplicationNotificationTypeMapCommand { ClientApplicationId = clientApplicationId, NotificationTypeId = notificationTypeId };
            await mediator.Send(command);
            return NoContent();
        }
    }
}