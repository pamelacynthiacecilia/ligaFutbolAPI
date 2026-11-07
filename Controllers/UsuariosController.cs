using LigaDeJugadoresAPI.DTOs.Usuarios;
using LigaDeJugadoresAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LigaDeJugadoresAPI.Controllers;

[ApiController]
[Route("api/usuarios")]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioService _usuarioService;

    public UsuariosController(IUsuarioService usuarioService)
    {
        _usuarioService = usuarioService;
    }

    // Crear usuarios: solo administradores autenticados
    [Authorize(Roles = "Admin")]
    [HttpPost("crear")]
    public async Task<ActionResult<UsuarioDto>> CrearUsuario(
        [FromBody] CrearUsuarioDto dto)
    {
        try
        {
            var usuario = await _usuarioService.CrearUsuarioAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                usuario
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }

 // Cambiar la contraseña del usuario autenticado
[Authorize]
[HttpPut("cambiarPassword")]
public async Task<IActionResult> CambiarPassword(
    [FromBody] CambiarPasswordDto dto)
{
    // Obtener el ID del usuario desde el JWT
    var idClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

    if (!int.TryParse(idClaim, out int idUsuario))
    {
        return Unauthorized("No se pudo identificar al usuario.");
    }

    try
    {
        await _usuarioService.CambiarPasswordAsync(idUsuario, dto);

        return Ok("La contraseña se actualizó correctamente.");
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
}

// Restablecer la contraseña de un usuario: solo administradores
[Authorize(Roles = "Admin")]
[HttpPut("restablecerPassword/{id:int}")]
public async Task<IActionResult> RestablecerPassword(
    int id,
    [FromBody] RestablecerPasswordDto dto)
{
    try
    {
        await _usuarioService.RestablecerPasswordAsync(id, dto);

        return Ok("La contraseña del usuario se restableció correctamente.");
    }
    catch (KeyNotFoundException ex)
    {
        return NotFound(ex.Message);
    }
}
}