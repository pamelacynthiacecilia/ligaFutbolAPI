using LigaDeJugadoresAPI.DTOs.Auth;

namespace LigaDeJugadoresAPI.Interfaces;

public interface IAuthService
{
    Task<LoginResponse?> LoginAsync(LoginDto loginDto);
}