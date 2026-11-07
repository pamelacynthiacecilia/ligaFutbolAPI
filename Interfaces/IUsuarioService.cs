using LigaDeJugadoresAPI.DTOs.Usuarios;

namespace LigaDeJugadoresAPI.Interfaces;

public interface IUsuarioService
{
    Task<UsuarioDto> CrearUsuarioAsync(CrearUsuarioDto dto);
    Task<bool> ExistenUsuariosAsync();

    Task<bool> CambiarPasswordAsync(
    int idUsuario,
    CambiarPasswordDto dto);

    Task<bool> RestablecerPasswordAsync(
    int idUsuario,
    RestablecerPasswordDto dto);
    
}