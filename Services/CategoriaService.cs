using LigaDeJugadoresAPI.Data;
using LigaDeJugadoresAPI.DTOs.Categorias;
using LigaDeJugadoresAPI.Interfaces;
using Microsoft.EntityFrameworkCore;
using LigaDeJugadoresAPI.Models;
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

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
{
    return await _context.Categoria
        .AsNoTracking()
        .Where(c => c.Id == id)
        .Select(c => new CategoriaDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            EdadMin = c.EdadMin,
            EdadMax = c.EdadMax
        })
        .FirstOrDefaultAsync();
}

public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto dto)
{
    var nombre = dto.Nombre?.Trim() ?? "";

    ValidarDatos(nombre, dto.EdadMin, dto.EdadMax);

    var existe = await _context.Categoria
        .AnyAsync(c => c.Nombre.ToLower() == nombre.ToLower());

    if (existe)
        throw new InvalidOperationException(
            "Ya existe una categoría con ese nombre.");

    var categoria = new Categoria
    {
        Nombre = nombre,
        EdadMin = dto.EdadMin,
        EdadMax = dto.EdadMax
    };

    _context.Categoria.Add(categoria);
    await _context.SaveChangesAsync();

    return MapearDto(categoria);
}

public async Task<CategoriaDto?> ActualizarAsync(
    int id,
    ActualizarCategoriaDto dto)
{
    var categoria = await _context.Categoria.FindAsync(id);

    if (categoria == null)
        return null;

    var nombre = dto.Nombre?.Trim() ?? "";

    ValidarDatos(nombre, dto.EdadMin, dto.EdadMax);

    var existe = await _context.Categoria.AnyAsync(c =>
        c.Id != id &&
        c.Nombre.ToLower() == nombre.ToLower());

    if (existe)
        throw new InvalidOperationException(
            "Ya existe otra categoría con ese nombre.");

    categoria.Nombre = nombre;
    categoria.EdadMin = dto.EdadMin;
    categoria.EdadMax = dto.EdadMax;

    await _context.SaveChangesAsync();

    return MapearDto(categoria);
}

public async Task<bool> EliminarAsync(int id)
{
    var categoria = await _context.Categoria.FindAsync(id);

    if (categoria == null)
        return false;

    var tieneJugadores = await _context.Jugadors
        .AnyAsync(j => j.IdCategoria == id);

    if (tieneJugadores)
        throw new InvalidOperationException(
            "No se puede eliminar la categoría porque tiene jugadores asociados.");

    _context.Categoria.Remove(categoria);
    await _context.SaveChangesAsync();

    return true;
}

private static void ValidarDatos(
    string nombre,
    int edadMin,
    int edadMax)
{
    if (string.IsNullOrWhiteSpace(nombre))
        throw new ArgumentException(
            "El nombre de la categoría es obligatorio.");

    if (nombre.Length > 50)
        throw new ArgumentException(
            "El nombre no puede superar los 50 caracteres.");

    if (edadMin < 0 || edadMax < 0 ||
        edadMin > 120 || edadMax > 120)
        throw new ArgumentException(
            "Las edades deben estar entre 0 y 120.");

    if (edadMin > edadMax)
        throw new ArgumentException(
            "La edad mínima no puede superar la edad máxima.");
}

private static CategoriaDto MapearDto(Categoria categoria)
{
    return new CategoriaDto
    {
        Id = categoria.Id,
        Nombre = categoria.Nombre,
        EdadMin = categoria.EdadMin,
        EdadMax = categoria.EdadMax
    };
}
}