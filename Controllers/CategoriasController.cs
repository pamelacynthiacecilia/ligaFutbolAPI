using LigaDeJugadoresAPI.DTOs.Categorias;
using LigaDeJugadoresAPI.Interfaces;
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

    [HttpGet]
    public async Task<ActionResult<List<CategoriaDto>>> ObtenerTodas()
    {
        var categorias = await _categoriaService.ObtenerTodasAsync();

        return Ok(categorias);
    }
}