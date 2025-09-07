// File Path: Application/Common/Interfaces/ICurrentUserService.cs
using System;

namespace Application.Common.Interfaces
{
    public interface ICurrentUserService
    {
        string? UserId { get; }
        string? UserName { get; }
        string? Email { get; }
        bool IsAuthenticated { get; }
        string GetUserIdOrDefault();
    }
}