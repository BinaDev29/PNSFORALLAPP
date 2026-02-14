// File Path: API/Controllers/EmailTemplateController.cs
using Application.CQRS.EmailTemplate.Commands;
using Application.CQRS.EmailTemplate.Queries;
using Application.DTO.EmailTemplate;
using Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmailTemplateController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EmailTemplateController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/EmailTemplate
        [HttpGet]
        [ProducesResponseType(typeof(List<EmailTemplateDto>), 200)]
        public async Task<ActionResult<List<EmailTemplateDto>>> Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var isAdmin = User.IsInRole("Admin");

            var query = new GetEmailTemplatesListQuery
            {
                UserId = userId,
                IsAdmin = isAdmin
            };
            var templates = await _mediator.Send(query);
            return Ok(templates);
        }

        // GET: api/EmailTemplate/5
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(EmailTemplateDto), 200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<EmailTemplateDto>> Get(Guid id)
        {
            var query = new GetEmailTemplateDetailQuery { Id = id };
            var template = await _mediator.Send(query);

            // FIX: Return a 404 Not Found if the template is null.
            if (template == null)
            {
                return NotFound();
            }

            return Ok(template);
        }

        // POST: api/EmailTemplate
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 201)] // FIX: Change status code to 201 Created.
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateEmailTemplateDto createEmailTemplateDto)
        {
            var command = new CreateEmailTemplateCommand { CreateEmailTemplateDto = createEmailTemplateDto };
            var response = await _mediator.Send(command);

            // FIX: Check for validation errors from the MediatR handler.
            if (!response.Success)
            {
                return BadRequest(response.Errors);
            }

            // FIX: Return 201 Created with the location of the new resource.
            return CreatedAtAction(nameof(Get), new { id = response.Id }, response);
        }

        // PUT: api/EmailTemplate/5
        [HttpPut("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put(Guid id, [FromBody] UpdateEmailTemplateDto updateEmailTemplateDto)
        {
            updateEmailTemplateDto.Id = id;
            var command = new UpdateEmailTemplateCommand { UpdateEmailTemplateDto = updateEmailTemplateDto };

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
                return BadRequest(new { Message = ex.Message });
            }
        }

        // DELETE: api/EmailTemplate/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteEmailTemplateCommand { Id = id };

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