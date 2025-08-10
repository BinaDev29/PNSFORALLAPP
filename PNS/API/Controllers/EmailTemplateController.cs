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

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            var query = new GetEmailTemplatesListQuery();
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
            return Ok(template);
        }

        // POST: api/EmailTemplate
        [HttpPost]
        [ProducesResponseType(typeof(BaseCommandResponse), 200)]
        [ProducesResponseType(400)]
        public async Task<ActionResult<BaseCommandResponse>> Post([FromBody] CreateEmailTemplateDto createEmailTemplateDto)
        {
            var command = new CreateEmailTemplateCommand { CreateEmailTemplateDto = createEmailTemplateDto };
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        // PUT: api/EmailTemplate
        [HttpPut]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Put([FromBody] UpdateEmailTemplateDto updateEmailTemplateDto)
        {
            var command = new UpdateEmailTemplateCommand { UpdateEmailTemplateDto = updateEmailTemplateDto };
            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/EmailTemplate/5
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<ActionResult> Delete(Guid id)
        {
            var command = new DeleteEmailTemplateCommand { Id = id };
            await _mediator.Send(command);
            return NoContent();
        }
    }
}