using LigaDeJugadoresAPI.Data;
using LigaDeJugadoresAPI.DTOs.Clubes;
using LigaDeJugadoresAPI.Interfaces;
using LigaDeJugadoresAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LigaDeJugadoresAPI.Services;

public class ClubesService : IClubesService
{
    private readonly LigaDbContext _context;
    private readonly IWebHostEnvironment _environment;
    public ClubesService(LigaDbContext context, IWebHostEnvironment environment)
    {
        _context = context;
        _environment = environment;
    }

public async Task<ClubDto> CrearClubAsync(CrearClubDto dto)
    {
        // Validar que el nombre no esté vacío
        if (string.IsNullOrWhiteSpace(dto.Nombre))
        {
            throw new ArgumentException(
                "El nombre del club es obligatorio.");
        }

        // Crear la entidad
        var club = new Clubfutbol
        {
            Nombre = dto.Nombre.Trim(),
            Escudo = dto.Escudo,
            Barrio = dto.Barrio,
            Localidad = dto.Localidad,
            FechaFundacion = dto.FechaFundacion,
            Estado = dto.Estado
        };

        // Agregar el club al contexto
        _context.Set<Clubfutbol>().Add(club);

        // Guardar los cambios en MySQL
        await _context.SaveChangesAsync();

        // Devolver los datos del club creado
        return new ClubDto
        {
            Id = club.Id,
            Nombre = club.Nombre,
            Escudo = club.Escudo,
            Barrio = club.Barrio,
            Localidad = club.Localidad,
            FechaFundacion = club.FechaFundacion,
            Estado = club.Estado
        };
    }

public async Task<ClubDto?> ActualizarClubAsync( int id,
    ActualizarClubDto dto)
{
    // Buscar el club en la base de datos
    var club = await _context.Set<Clubfutbol>().FindAsync(id);

    if (club == null)
    {
        return null;
    }

    // Validar el nombre
    if (string.IsNullOrWhiteSpace(dto.Nombre))
    {
        throw new ArgumentException(
            "El nombre del club es obligatorio.");
    }

    // Actualizar sus propiedades
    club.Nombre = dto.Nombre.Trim();
    club.Escudo = dto.Escudo;
    club.Barrio = dto.Barrio;
    club.Localidad = dto.Localidad;
    club.FechaFundacion = dto.FechaFundacion;
    club.Estado = dto.Estado;

    // Guardar los cambios
    await _context.SaveChangesAsync();

    // Devolver el club actualizado
    return new ClubDto
    {
        Id = club.Id,
        Nombre = club.Nombre,
        Escudo = club.Escudo,
        Barrio = club.Barrio,
        Localidad = club.Localidad,
        FechaFundacion = club.FechaFundacion,
        Estado = club.Estado
    };
}
public async Task<List<ClubDto>> ObtenerClubesAsync()
{
    return await _context.Set<Clubfutbol>()
        .AsNoTracking()
        .OrderBy(c => c.Nombre)
        .Select(c => new ClubDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Escudo = c.Escudo,
            Barrio = c.Barrio,
            Localidad = c.Localidad,
            FechaFundacion = c.FechaFundacion,
            Estado = c.Estado
        })
        .ToListAsync();
}

public async Task<ClubDto?> ObtenerClubPorIdAsync(int id)
{
    return await _context.Set<Clubfutbol>()
        .AsNoTracking()
        .Where(c => c.Id == id)
        .Select(c => new ClubDto
        {
            Id = c.Id,
            Nombre = c.Nombre,
            Escudo = c.Escudo,
            Barrio = c.Barrio,
            Localidad = c.Localidad,
            FechaFundacion = c.FechaFundacion,
            Estado = c.Estado
        })
        .FirstOrDefaultAsync();
} 

public async Task<bool> EliminarClubAsync(int id)
{
    var club = await _context.Set<Clubfutbol>().FindAsync(id);

    if (club == null)
    {
        return false;
    }

    // Baja lógica: no elimina el registro
    club.Estado = false;

    await _context.SaveChangesAsync();

    return true;
}

public async Task<ClubDto?> SubirEscudoAsync(int id, IFormFile archivo)
{
    // 1. Buscar el club
    var club = await _context.Set<Clubfutbol>().FindAsync(id);

    if (club == null)
        return null;

    // 2. Validar el archivo
    if (archivo == null || archivo.Length == 0)
        throw new ArgumentException("Debe seleccionar una imagen.");

    const long tamañoMaximo = 2 * 1024 * 1024; // 2 MB

    if (archivo.Length > tamañoMaximo)
        throw new ArgumentException("La imagen no puede superar los 2 MB.");

    var extension = Path.GetExtension(archivo.FileName).ToLowerInvariant();

    if (extension != ".png" && extension != ".jpg" && extension != ".jpeg")
        throw new ArgumentException("Solo se permiten imágenes PNG o JPG.");

    // 3. Validar la firma del archivo
    byte[] encabezado = new byte[8];

    using (var stream = archivo.OpenReadStream())
    {
        var leidos = await stream.ReadAsync(encabezado.AsMemory());
        if (leidos < 8)
            throw new ArgumentException("El archivo no es una imagen válida.");
    }

    bool esPng = encabezado.SequenceEqual(
        new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });

    bool esJpeg = encabezado[0] == 0xFF &&
                  encabezado[1] == 0xD8 &&
                  encabezado[2] == 0xFF;

    if ((extension == ".png" && !esPng) ||
        ((extension == ".jpg" || extension == ".jpeg") && !esJpeg))
        throw new ArgumentException("El contenido no coincide con el formato de imagen.");

    // 4. Preparar la carpeta de destino
    var carpeta = Path.Combine(
        _environment.WebRootPath,
        "Uploads",
        "ClubesEscudos");

    Directory.CreateDirectory(carpeta);

    // 5. Generar un nombre único
    var nombreArchivo = $"{Guid.NewGuid():N}{extension}";
    var rutaFisica = Path.Combine(carpeta, nombreArchivo);
    var rutaRelativa = $"/Uploads/ClubesEscudos/{nombreArchivo}";

    // 6. Guardar la imagen
    using (var stream = new FileStream(
        rutaFisica, FileMode.CreateNew))
    {
        await archivo.CopyToAsync(stream);
    }

    // 7. Actualizar el club en MySQL
    var escudoAnterior = club.Escudo;
    club.Escudo = rutaRelativa;

    try
    {
        await _context.SaveChangesAsync();
    }
    catch
    {
        // Si falla la base de datos, eliminar la imagen recién subida
        File.Delete(rutaFisica);
        throw;
    }

    // 8. Eliminar el escudo anterior, si pertenece a nuestra carpeta
    if (!string.IsNullOrWhiteSpace(escudoAnterior) &&
        escudoAnterior.StartsWith(
            "/Uploads/ClubesEscudos/",
            StringComparison.OrdinalIgnoreCase))
    {
        var nombreAnterior = Path.GetFileName(escudoAnterior);
        var rutaAnterior = Path.Combine(carpeta, nombreAnterior);

        if (File.Exists(rutaAnterior) && rutaAnterior != rutaFisica)
        {
            try
            {
                File.Delete(rutaAnterior);
            }
            catch (IOException)
            {
                // La imagen anterior podrá limpiarse posteriormente
            }
            catch (UnauthorizedAccessException)
            {
                // La imagen anterior podrá limpiarse posteriormente
            }
        }
    }

    // 9. Devolver el club actualizado
    return new ClubDto
    {
        Id = club.Id,
        Nombre = club.Nombre,
        Escudo = club.Escudo,
        Barrio = club.Barrio,
        Localidad = club.Localidad,
        FechaFundacion = club.FechaFundacion,
        Estado = club.Estado
    };
}
}