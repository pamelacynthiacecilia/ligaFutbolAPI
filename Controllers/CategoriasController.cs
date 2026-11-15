using LigaDeJugadoresAPI.DTOs.Categorias;
using LigaDeJugadoresAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace LigaDeJugadoresAPI.Controllers;

[ApiController]
[Route("api/categorias")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet("obtenerTodos")]
    public async Task<ActionResult<List<CategoriaDto>>> ObtenerTodas()
    {
        var categorias = await _categoriaService.ObtenerTodasAsync();

        return Ok(categorias);
    }

// GET: api/categorias/obtener/5
    [HttpGet("obtener/{id:int}")]
    public async Task<ActionResult<CategoriaDto>> ObtenerPorId(int id)
    {
        var categoria = await _categoriaService.ObtenerPorIdAsync(id);

        if (categoria == null)
            return NotFound(new { mensaje = "La categoría no existe." });

        return Ok(categoria);
    }

    // POST: api/categorias/crear
    [HttpPost("crear")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoriaDto>> Crear(
        [FromBody] CrearCategoriaDto dto)
    {
        try
        {
            var categoria = await _categoriaService.CrearAsync(dto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = categoria.Id },
                categoria);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // PUT: api/categorias/actualizar/5
    [HttpPut("actualizar/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CategoriaDto>> Actualizar(
        int id,
        [FromBody] ActualizarCategoriaDto dto)
    {
        try
        {
            var categoria = await _categoriaService.ActualizarAsync(id, dto);

            if (categoria == null)
                return NotFound(new { mensaje = "La categoría no existe." });

            return Ok(categoria);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensaje = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

    // DELETE: api/categorias/eliminar/5
    [HttpDelete("eliminar/{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminado = await _categoriaService.EliminarAsync(id);

            if (!eliminado)
                return NotFound(new { mensaje = "La categoría no existe." });

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { mensaje = ex.Message });
        }
    }

}