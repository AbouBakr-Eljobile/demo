using GovContracts.Api.Application.DTOs.Auth;
using GovContracts.Api.Application.Interfaces;

namespace GovContracts.Api.Application.Services;

public class AuthService : IAuthService
{
    public LoginResponseDto? Login(LoginRequestDto request)
    {
        var isValid = request.Username.Equals("admin", StringComparison.OrdinalIgnoreCase)
            && request.Password == "Admin@123";

        if (!isValid)
        {
            return null;
        }

        return new LoginResponseDto
        {
            Token = $"mock-token-{Guid.NewGuid():N}",
            DisplayName = "موظف نظام العقود"
        };
    }
}
