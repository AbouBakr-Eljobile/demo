using GovContracts.Api.Application.DTOs.Auth;
using GovContracts.Api.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GovContracts.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public ActionResult<LoginResponseDto> Login([FromBody] LoginRequestDto request)
    {
        var response = _authService.Login(request);
        if (response is null)
        {
            return Unauthorized(new { message = "بيانات الدخول غير صحيحة" });
        }

        return Ok(response);
    }
}
