using LigaDeJugadoresAPI.DTOs.Usuarios;
using LigaDeJugadoresAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
    [HttpPost]
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

 
}