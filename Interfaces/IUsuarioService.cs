using LigaDeJugadoresAPI.DTOs.Usuarios;

namespace LigaDeJugadoresAPI.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto> CrearUsuarioAsync(CrearUsuarioDto dto);
    Task<bool> ExistenUsuariosAsync();

    
}