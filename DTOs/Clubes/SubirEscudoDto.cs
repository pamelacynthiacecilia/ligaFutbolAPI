using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace LigaDeJugadoresAPI.DTOs.Clubes;

public class SubirEscudoDto
{
    [Required]
    public IFormFile Archivo { get; set; } = null!;
}