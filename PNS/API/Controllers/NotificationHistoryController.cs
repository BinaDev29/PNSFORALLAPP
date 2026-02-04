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
using Application.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotificationHistoryController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/NotificationHistory
        [HttpGet]
        [ProducesResponseType(typeof(List<NotificationHistoryDto>), 200)]
        public async Task<ActionResult<List<NotificationHistoryDto>>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            var query = new GetNotificationHistoriesListQuery
            {
                UserId = userId,
                IsAdmin = isAdmin
            };
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

            // FIX: Return a 404 Not Found if the history item is null.
            if (history == null)
            {
                return NotFound();
            }

            return Ok(history);
        }

        // POST: api/NotificationHistory
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 201)] // FIX: Change status code to 201 Created.
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateNotificationHistoryDto createNotificationHistoryDto)
        {
            var command = new CreateNotificationHistoryCommand { CreateNotificationHistoryDto = createNotificationHistoryDto };
            var response = await _mediator.Send(command);

            // FIX: Check for validation errors from the MediatR handler.
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            // FIX: Return 201 Created with the location of the new resource.
            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }
    }
}