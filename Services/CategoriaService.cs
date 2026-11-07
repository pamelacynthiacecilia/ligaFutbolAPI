using LigaDeJugadoresAPI.Data;
using LigaDeJugadoresAPI.DTOs.Categorias;
using LigaDeJugadoresAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LigaDeJugadoresAPI.Services;

public class CategoriaService : ICategoriaService
{
    private readonly LigaDbContext _context;

    public CategoriaService(LigaDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDto>> ObtenerTodasAsync()
    {
        return await _context.Categoria
            .AsNoTracking()
            .OrderBy(c => c.Id)
            .Select(c => new CategoriaDto
            {
                Id = c.Id,
                Nombre = c.Nombre,
                EdadMin = c.EdadMin,
                EdadMax = c.EdadMax
            })
            .ToListAsync();
    }
}