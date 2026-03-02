using GovContracts.Api.Application.DTOs.Auth;

namespace GovContracts.Api.Application.Interfaces;

public interface IAuthService
{
    LoginResponseDto? Login(LoginRequestDto request);
}
