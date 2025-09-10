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

            // FIX: Explicitly check if the map was not found
            if (map == null)
            {
                return NotFound();
            }

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

            // FIX: Check for validation errors from the MediatR handler
            if (!response.Success)
            {
                // Assuming BaseCommandResponse has a property for validation errors
                return BadRequest(response.Errors);
            }

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

            // FIX: Wrap in a try-catch to handle potential NotFound exceptions from the handler
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Application.Exceptions.NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/ApplicationNotificationTypeMap/{clientApplicationId}/{notificationTypeId}
        [HttpDelete("{clientApplicationId}/{notificationTypeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid clientApplicationId, Guid notificationTypeId)
        {
            var command = new DeleteApplicationNotificationTypeMapCommand { ClientApplicationId = clientApplicationId, NotificationTypeId = notificationTypeId };

            // FIX: Wrap in a try-catch to handle potential NotFound exceptions from the handler
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (Application.Exceptions.NotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                // For other potential exceptions, a BadRequest might be more appropriate than a 500 error
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}