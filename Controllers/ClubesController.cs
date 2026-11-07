using LigaDeJugadoresAPI.DTOs.Clubes;
using LigaDeJugadoresAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaDeJugadoresAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/clubes")]
public class ClubesController : ControllerBase
{
    private readonly IClubesService _clubesService;

    public ClubesController(IClubesService clubesService)
    {
        _clubesService = clubesService;
    }

    [Authorize(Roles = "Admin")]
    [HttpPost("crear")]
    public async Task<ActionResult<ClubDto>> CrearClub(
        [FromBody] CrearClubDto dto)
    {
        try
        { 
            var club = await _clubesService.CrearClubAsync(dto);

            return StatusCode(
                StatusCodes.Status201Created,
                club
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

[Authorize(Roles = "Admin")]
[HttpPut("actualizar/{id}")]
public async Task<ActionResult<ClubDto>> ActualizarClub(
    int id,
    [FromBody] ActualizarClubDto dto)
{
    try
    {
        var club = await _clubesService.ActualizarClubAsync(id, dto);

        if (club == null)
        {
            return NotFound("El club no existe.");
        }

        return Ok(club);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
}

[Authorize(Roles = "Admin")]
[HttpGet("obtenerTodos")]
public async Task<ActionResult<List<ClubDto>>> ObtenerClubes()
{
    var clubes = await _clubesService.ObtenerClubesAsync();

    return Ok(clubes);
}
[Authorize(Roles = "Admin")]
[HttpGet("obtener/{id:int}")]
public async Task<ActionResult<ClubDto>> ObtenerClubPorId(int id)
{
    var club = await _clubesService.ObtenerClubPorIdAsync(id);

    if (club == null)
    {
        return NotFound("El club no existe.");
    }

    return Ok(club);
}

[Authorize(Roles = "Admin")]
[HttpDelete("eliminar/{id:int}")]
public async Task<IActionResult> EliminarClub(int id)
{
    var eliminado = await _clubesService.EliminarClubAsync(id);

    if (!eliminado)
    {
        return NotFound("El club no existe.");
    }

    return NoContent();
}

[Authorize(Roles = "Admin")]
[HttpPost("{id:int}/escudo")]
[Consumes("multipart/form-data")]
public async Task<ActionResult<ClubDto>> SubirEscudo(
    int id,
    [FromForm] SubirEscudoDto dto)
{
    try
    {
        var club = await _clubesService.SubirEscudoAsync(
            id, dto.Archivo);

        if (club == null)
        {
            return NotFound("El club no existe.");
        }

        return Ok(club);
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ex.Message);
    }
}
}