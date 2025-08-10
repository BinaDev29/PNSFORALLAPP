// File Path: API/Controllers/ApplicationNotificationTypeMapController.cs
using Application.CQRS.ApplicationNotificationTypeMap.Commands;
using Application.CQRS.ApplicationNotificationTypeMap.Queries;
using Application.DTO.ApplicationNotificationTypeMap;
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
    public class ApplicationNotificationTypeMapController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ApplicationNotificationTypeMapController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/ApplicationNotificationTypeMap
        [HttpGet]
        [ProducesResponseType(typeof(List<ApplicationNotificationTypeMapDto>), 200)]
        public async Task<ActionResult<List<ApplicationNotificationTypeMapDto>>> Get()
        {
            var query = new GetApplicationNotificationTypeMapsListQuery();
            var maps = await _mediator.Send(query);
            return Ok(maps);
        }

        // GET: api/ApplicationNotificationTypeMap/{clientApplicationId}/{notificationTypeId}
        [HttpGet("{clientApplicationId}/{notificationTypeId}")]
        [ProducesResponseType(typeof(ApplicationNotificationTypeMapDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ApplicationNotificationTypeMapDto>> Get(Guid clientApplicationId, Guid notificationTypeId)
        {
            var query = new GetApplicationNotificationTypeMapDetailQuery { ClientApplicationId = clientApplicationId, NotificationTypeId = notificationTypeId };
            var map = await _mediator.Send(query);
            return Ok(map);
        }

        // POST: api/ApplicationNotificationTypeMap
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateApplicationNotificationTypeMapDto createApplicationNotificationTypeMapDto)
        {
            var command = new CreateApplicationNotificationTypeMapCommand { CreateApplicationNotificationTypeMapDto = createApplicationNotificationTypeMapDto };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT: api/ApplicationNotificationTypeMap
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdateApplicationNotificationTypeMapDto updateApplicationNotificationTypeMapDto)
        {
            var command = new UpdateApplicationNotificationTypeMapCommand { UpdateApplicationNotificationTypeMapDto = updateApplicationNotificationTypeMapDto };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/ApplicationNotificationTypeMap/{clientApplicationId}/{notificationTypeId}
        [HttpDelete("{clientApplicationId}/{notificationTypeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid clientApplicationId, Guid notificationTypeId)
        {
            var command = new DeleteApplicationNotificationTypeMapCommand { ClientApplicationId = clientApplicationId, NotificationTypeId = notificationTypeId };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}