// File Path: API/Controllers/ClientApplicationController.cs
using Application.CQRS.ClientApplication.Commands;
using Application.CQRS.ClientApplication.Queries;
using Application.DTO.ClientApplication;
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
    public class ClientApplicationController(IMediator mediator) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/ClientApplication
        [HttpGet]
        [ProducesResponseType(typeof(List<ClientApplicationDto>), 200)]
        public async Task<ActionResult<List<ClientApplicationDto>>> Get()
        {
            var query = new GetClientApplicationsListQuery();
            var applications = await _mediator.Send(query);
            return Ok(applications);
        }

        // GET: api/ClientApplication/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ClientApplicationDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<ClientApplicationDto>> Get(Guid id)
        {
            var query = new GetClientApplicationDetailQuery { Id = id };
            var application = await _mediator.Send(query);
            return Ok(application);
        }

        // POST: api/ClientApplication
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateClientApplicationDto createClientApplicationDto)
        {
            // ? ?????? ???? ??????? Command?? ??? ???? ??? ?
            var command = new CreateClientApplicationCommand(createClientApplicationDto);
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT: api/ClientApplication
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdateClientApplicationDto updateClientApplicationDto)
        {
            var command = new UpdateClientApplicationCommand { UpdateClientApplicationDto = updateClientApplicationDto };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/ClientApplication/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteClientApplicationCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}