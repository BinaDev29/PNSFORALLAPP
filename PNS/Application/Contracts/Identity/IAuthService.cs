using Application.DTO.Auth;

namespace Application.Contracts.Identity;

public interface IAuthService
{
    Task<AuthResponse> Login(AuthRequest request);
    Task<AuthResponse> Register(RegistrationRequest request);
    Task<bool> UpdateProfile(string userId, UserProfileUpdateDto request);
    Task<bool> ChangePassword(string userId, ChangePasswordDto request);
}
