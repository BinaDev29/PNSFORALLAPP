using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.EmailTemplate.Commands;
using Application.CQRS.EmailTemplate.Queries;
using Application.DTO.EmailTemplate;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailTemplateController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetEmailTemplates()
        {
            var query = new GetEmailTemplatesListQuery();
            var templates = await mediator.Send(query);
            return Ok(templates);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEmailTemplate(Guid id)
        {
            var query = new GetEmailTemplateDetailQuery { Id = id };
            var template = await mediator.Send(query);
            return Ok(template);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEmailTemplate([FromBody] CreateEmailTemplateDto dto)
        {
            var command = new CreateEmailTemplateCommand { CreateEmailTemplateDto = dto };
            var response = await mediator.Send(command);
            return CreatedAtAction(nameof(GetEmailTemplate), new { id = response.Id }, response);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateEmailTemplate([FromBody] UpdateEmailTemplateDto dto)
        {
            var command = new UpdateEmailTemplateCommand { UpdateEmailTemplateDto = dto };
            await mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmailTemplate(Guid id)
        {
            var command = new DeleteEmailTemplateCommand { Id = id };
            await mediator.Send(command);
            return NoContent();
        }
    }
}