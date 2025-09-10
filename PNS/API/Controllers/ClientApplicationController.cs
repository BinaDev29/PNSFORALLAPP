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
using Application.Exceptions;

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

            // FIX: Return a 404 Not Found if the application is null.
            if (application == null)
            {
                return NotFound();
            }

            return Ok(application);
        }

        // POST: api/ClientApplication
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 201)] // FIX: Changed to 201 Created for resource creation.
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateClientApplicationDto createClientApplicationDto)
        {
            var command = new CreateClientApplicationCommand(createClientApplicationDto);
            var response = await _mediator.Send(command);

            // FIX: Check for validation errors from the MediatR handler.
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            // FIX: Return 201 Created with the location of the new resource.
            // Assuming Id is the unique identifier of the newly created application.
            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        // PUT: api/ClientApplication
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdateClientApplicationDto updateClientApplicationDto)
        {
            var command = new UpdateClientApplicationCommand { UpdateClientApplicationDto = updateClientApplicationDto };

            // FIX: Wrap in a try-catch to handle potential NotFound and Validation exceptions.
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
                // A generic catch-all for other types of errors.
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/ClientApplication/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteClientApplicationCommand { Id = id };

            // FIX: Wrap in a try-catch to handle potential NotFound exceptions.
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