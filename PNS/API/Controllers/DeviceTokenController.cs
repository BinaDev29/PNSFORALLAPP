using Application.Contracts.IRepository;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DeviceTokenController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeviceTokenController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterToken([FromBody] RegisterTokenRequest request)
        {
            var existing = await _unitOfWork.DeviceTokens.GetAll();
            var token = existing.FirstOrDefault(t => t.Token == request.Token && t.ClientApplicationId == request.ClientApplicationId);

            if (token == null)
            {
                token = new DeviceToken
                {
                    Token = request.Token,
                    ClientApplicationId = request.ClientApplicationId,
                    UserId = request.UserId,
                    DeviceType = request.DeviceType,
                    LastUpdated = DateTime.UtcNow
                };
                await _unitOfWork.DeviceTokens.Add(token);
            }
            else
            {
                token.UserId = request.UserId;
                token.DeviceType = request.DeviceType;
                token.LastUpdated = DateTime.UtcNow;
                await _unitOfWork.DeviceTokens.Update(token);
            }

            await _unitOfWork.Save();
            return Ok(new { Message = "Token registered successfully" });
        }

        public class RegisterTokenRequest
        {
            public string Token { get; set; } = string.Empty;
            public Guid ClientApplicationId { get; set; }
            public string? UserId { get; set; }
            public string? DeviceType { get; set; }
        }
    }
}
