using MediatR;
using Microsoft.AspNetCore.Mvc;
using Application.CQRS.NotificationHistory.Commands;
using Application.CQRS.NotificationHistory.Queries;
using Application.DTO.NotificationHistory;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationHistoryController(IMediator mediator) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetNotificationHistories()
        {
            var query = new GetNotificationHistoriesListQuery();
            var histories = await mediator.Send(query);
            return Ok(histories);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotificationHistory(Guid id)
        {
            var query = new GetNotificationHistoryDetailQuery { Id = id };
            var history = await mediator.Send(query);
            return Ok(history);
        }

        [HttpPost]
        public async Task<IActionResult> CreateNotificationHistory([FromBody] CreateNotificationHistoryDto dto)
        {
            var command = new CreateNotificationHistoryCommand { CreateNotificationHistoryDto = dto };
            var response = await mediator.Send(command);
            return Ok(response);
        }
    }
}