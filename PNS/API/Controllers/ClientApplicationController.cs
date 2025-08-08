using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.ClientApplication.Commands;
using Application.CQRS.ClientApplication.Queries;
using Application.DTO.ClientApplication;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientApplicationController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetClientApplications()
        {
            var query = new GetClientApplicationsListQuery();
            var applications = await mediator.Send(query);
            // ባዶ ሊስት ሲመጣ በቀጥታ መመለስ ትክክል ነው
            return Ok(applications);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientApplication(Guid id)
        {
            var query = new GetClientApplicationDetailQuery { Id = id };
            var application = await mediator.Send(query);

            // መረጃው null ከሆነ NotFound() መመለስ ትክክለኛ RESTful API ልምድ ነው
            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClientApplication([FromBody] CreateClientApplicationDto dto)
        {
            var command = new CreateClientApplicationCommand { CreateClientApplicationDto = dto };
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetClientApplication), new { id = response.Id }, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateClientApplication([FromBody] UpdateClientApplicationDto dto)
        {
            var command = new UpdateClientApplicationCommand { UpdateClientApplicationDto = dto };
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteClientApplication(Guid id)
        {
            var command = new DeleteClientApplicationCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}