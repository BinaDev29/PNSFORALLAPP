using Application.Contracts.Identity;
using Application.DTO.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
    {
        try
        {
            var response = await _authService.Login(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return Unauthorized(new { message = ex.Message });
        }
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegistrationRequest request)
    {
        try
        {
            var response = await _authService.Register(request);
            return Ok(response);
        }
        catch (Exception ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [Authorize]
    [HttpPut("profile")]
    public async Task<IActionResult> UpdateProfile([FromBody] UserProfileUpdateDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _authService.UpdateProfile(userId, request);
        if (result) return Ok(new { message = "Profile updated successfully" });

        return BadRequest(new { message = "Failed to update profile" });
    }

    [Authorize]
    [HttpPost("change-password")]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto request)
    {
        var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userId)) return Unauthorized();

        var result = await _authService.ChangePassword(userId, request);
        if (result) return Ok(new { message = "Password changed successfully" });

        return BadRequest(new { message = "Failed to change password. Ensure your current password is correct." });
    }
}
