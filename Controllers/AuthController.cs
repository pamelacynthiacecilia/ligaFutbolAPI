using LigaDeJugadoresAPI.DTOs.Auth;
using LigaDeJugadoresAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaDeJugadoresAPI.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [AllowAnonymous] //permite iniciar sesión sin tener previamente un token
    [HttpPost("login")]
    public async Task<ActionResult<LoginResponse>> Login(
        [FromBody] LoginDto loginDto)
    {
        var respuesta = await _authService.LoginAsync(loginDto);

        if (respuesta == null)
        {
            return Unauthorized("Credenciales incorrectas.");
        }

        return Ok(respuesta);
    }
}