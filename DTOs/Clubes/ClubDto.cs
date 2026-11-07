namespace LigaDeJugadoresAPI.DTOs.Clubes;

public class ClubDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public string? Escudo { get; set; }

    public string? Barrio { get; set; }

    public string? Localidad { get; set; }

    public DateOnly? FechaFundacion { get; set; }

    public bool Estado { get; set; }
}