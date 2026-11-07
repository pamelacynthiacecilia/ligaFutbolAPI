using LigaDeJugadoresAPI.DTOs.Clubes;
namespace LigaDeJugadoresAPI.Interfaces;

public interface IClubesService
{
    Task<ClubDto> CrearClubAsync(CrearClubDto dto);
    Task<ClubDto?> ActualizarClubAsync(int id, ActualizarClubDto dto);
    Task<List<ClubDto>> ObtenerClubesAsync();
    Task<ClubDto?> ObtenerClubPorIdAsync(int id);
    Task<bool> EliminarClubAsync(int id);
    Task<ClubDto?> SubirEscudoAsync(int id, IFormFile archivo);
}