using LigaDeJugadoresAPI.DTOs.Categorias;

namespace LigaDeJugadoresAPI.Interfaces;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> ObtenerTodasAsync();
}