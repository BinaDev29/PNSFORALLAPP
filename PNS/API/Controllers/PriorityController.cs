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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PriorityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PriorityController(IMediator mediator)
        {
            _mediator = mediator;
        }

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
            return Ok(priority);
        }

        // POST: api/Priority
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreatePriorityDto createPriorityDto)
        {
            var command = new CreatePriorityCommand { CreatePriorityDto = createPriorityDto };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT: api/Priority
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdatePriorityDto updatePriorityDto)
        {
            var command = new UpdatePriorityCommand { UpdatePriorityDto = updatePriorityDto };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/Priority/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeletePriorityCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}