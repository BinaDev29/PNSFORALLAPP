// File Path: API/Controllers/SmsController.cs
using Application.CQRS.Sms.Commands;
using Application.DTO.Sms;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SmsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public SmsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Send SMS notification
        /// </summary>
        /// <param name="sendSmsDto">SMS notification details</param>
        /// <returns>Response indicating success or failure</returns>
        [HttpPost("send")]
        public async Task<IActionResult> SendSms([FromBody] SendSmsNotificationDto sendSmsDto)
        {
            try
            {
                var command = new SendSmsCommand { SendSmsDto = sendSmsDto };
                var response = await _mediator.Send(command);

                if (response.Success)
                {
                    return Ok(response);
                }
                else
                {
                    return BadRequest(response);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }

        /// <summary>
        /// Get SMS sending status
        /// </summary>
        /// <param name="notificationId">Notification ID</param>
        /// <returns>SMS status information</returns>
        [HttpGet("status/{notificationId}")]
        public async Task<IActionResult> GetSmsStatus(Guid notificationId)
        {
            try
            {
                // TODO: Implement status retrieval logic
                return Ok(new { notificationId, status = "Pending implementation" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Internal server error", error = ex.Message });
            }
        }
    }
}