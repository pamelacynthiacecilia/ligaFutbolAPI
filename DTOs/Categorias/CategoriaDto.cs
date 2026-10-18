namespace LigaDeJugadoresAPI.DTOs.Categorias;

public class CategoriaDto
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int EdadMin { get; set; }

    public int EdadMax { get; set; }
}