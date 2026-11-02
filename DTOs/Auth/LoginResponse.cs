namespace LigaDeJugadoresAPI.DTOs.Auth;

public class LoginResponse
{
    public string Token { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;
    public string Rol { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;
  
}