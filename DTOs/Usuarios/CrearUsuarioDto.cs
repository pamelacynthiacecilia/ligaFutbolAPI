using System.ComponentModel.DataAnnotations;

namespace LigaDeJugadoresAPI.DTOs.Usuarios;

public class CrearUsuarioDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(8)]
    public string Password { get; set; } = string.Empty;


    [Required]
    public string Nombre { get; set; } = string.Empty;

    [Required]
    public string Apellido { get; set; } = string.Empty;
    [Required]
    public string Rol { get; set; } = string.Empty;
}