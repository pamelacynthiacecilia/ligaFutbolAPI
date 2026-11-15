using System.ComponentModel.DataAnnotations;

namespace LigaDeJugadoresAPI.DTOs.Categorias;

public class ActualizarCategoriaDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(50)]
    public string Nombre { get; set; } = string.Empty;

    [Range(0, 120)]
    public int EdadMin { get; set; }

    [Range(0, 120)]
    public int EdadMax { get; set; }
}