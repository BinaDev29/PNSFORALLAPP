using Application.Contracts.Identity;
using Application.DTO.Auth;
using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Infrastructure.Identity;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthService(
        UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    public async Task<AuthResponse> Login(AuthRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
        {
            throw new Exception("Invalid Username or Password"); // Better: Custom Not Found/Auth Exception
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

        if (!result.Succeeded)
        {
            throw new Exception("Invalid Username or Password");
        }

        var token = await GenerateToken(user);

        return new AuthResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName!,
            Email = user.Email!,
            Token = token,
            Roles = (await _userManager.GetRolesAsync(user)).ToList()
        };
    }

    public async Task<AuthResponse> Register(RegistrationRequest request)
    {
        var existingUser = await _userManager.FindByEmailAsync(request.Email);
        if (existingUser != null)
        {
            throw new Exception($"User with {request.Email} already exists");
        }

        var user = new AppUser
        {
            Email = request.Email,
            UserName = request.UserName,
            FirstName = request.FirstName,
            LastName = request.LastName,
            EmailConfirmed = true 
        };

        var result = await _userManager.CreateAsync(user, request.Password);

        if (!result.Succeeded)
        {
            throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
        }

        // Assign 'User' role by default
        await _userManager.AddToRoleAsync(user, "User");

        var token = await GenerateToken(user);
        
        return new AuthResponse
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            UserName = user.UserName!,
            Email = user.Email!,
            Token = token,
            Roles = new List<string> { "User" }
        };
    }

    private async Task<string> GenerateToken(AppUser user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = jwtSettings["Key"] ?? "MySuperSecretKeyForDevelopmentOnly12345!"; // Fallback
        
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var roles = await _userManager.GetRolesAsync(user);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        // Add role claims
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var token = new JwtSecurityToken(
            issuer: jwtSettings["Issuer"],
            audience: jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings["DurationInMinutes"] ?? "60")),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public async Task<bool> UpdateProfile(string userId, UserProfileUpdateDto request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.UserName = request.Email; // Keep username synced with email for simplicity if preferred

        var result = await _userManager.UpdateAsync(user);
        return result.Succeeded;
    }

    public async Task<bool> ChangePassword(string userId, ChangePasswordDto request)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return false;

        var result = await _userManager.ChangePasswordAsync(user, request.CurrentPassword, request.NewPassword);
        return result.Succeeded;
    }
}
