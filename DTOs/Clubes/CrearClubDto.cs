using System.ComponentModel.DataAnnotations;

namespace LigaDeJugadoresAPI.DTOs.Clubes;

public class CrearClubDto
{
    [Required]
    [MaxLength(100)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Escudo { get; set; }

    [MaxLength(255)]
    public string? Barrio { get; set; }

    [MaxLength(100)]
    public string? Localidad { get; set; }

    public DateOnly? FechaFundacion { get; set; }

    [Required]
    public bool Estado { get; set; }
}