using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.Priority.Commands;
using Application.CQRS.Priority.Queries;
using Application.DTO.Priority;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PriorityController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetPriorities()
        {
            var query = new GetPrioritiesListQuery();
            var priorities = await mediator.Send(query);
            return Ok(priorities);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPriority(Guid id)
        {
            var query = new GetPriorityDetailQuery { Id = id };
            var priority = await mediator.Send(query);
            return Ok(priority);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePriority([FromBody] CreatePriorityDto dto)
        {
            var command = new CreatePriorityCommand { CreatePriorityDto = dto };
            var response = await mediator.Send(command);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePriority([FromBody] UpdatePriorityDto dto)
        {
            var command = new UpdatePriorityCommand { UpdatePriorityDto = dto };
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePriority(Guid id)
        {
            var command = new DeletePriorityCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}