// File Path: API/Controllers/PriorityController.cs
using Application.CQRS.Priority.Commands;
using Application.CQRS.Priority.Queries;
using Application.DTO.Priority;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Exceptions;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/Priority
        [HttpGet]
        [ProducesResponseType(typeof(List<PriorityDto>), 200)]
        public async Task<ActionResult<List<PriorityDto>>> Get()
        {
            var query = new GetPrioritiesListQuery();
            var priorities = await _mediator.Send(query);
            return Ok(priorities);
        }

        // GET: api/Priority/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PriorityDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<PriorityDto>> Get(Guid id)
        {
            var query = new GetPriorityDetailQuery { Id = id };
            var priority = await _mediator.Send(query);

            // FIX: Return a 404 Not Found if the priority is null.
            if (priority == null)
            {
                return NotFound();
            }

            return Ok(priority);
        }

        // POST: api/Priority
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 201)] // FIX: Change status code to 201 Created.
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreatePriorityDto createPriorityDto)
        {
            var command = new CreatePriorityCommand { CreatePriorityDto = createPriorityDto };
            var response = await _mediator.Send(command);

            // FIX: Check for validation errors from the MediatR handler.
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            // FIX: Return 201 Created with the location of the new resource.
            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        // PUT: api/Priority
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdatePriorityDto updatePriorityDto)
        {
            var command = new UpdatePriorityCommand { UpdatePriorityDto = updatePriorityDto };

            // FIX: Wrap in a try-catch to handle a potential NotFound exception.
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (NotFoundException) // Assuming a custom NotFoundException is thrown by the handler.
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/Priority/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeletePriorityCommand { Id = id };

            // FIX: Wrap in a try-catch to handle a potential NotFound exception.
            try
            {
                await _mediator.Send(command);
                return NoContent();
            }
            catch (NotFoundException) // Assuming a custom NotFoundException is thrown by the handler.
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message });
            }
        }
    }
}