using System;
using System.Collections.Generic;

namespace LigaDeJugadoresAPI.Models;

public partial class Jugador
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public DateOnly FechaNac { get; set; }

    public string? Dni { get; set; }

    public string DispPase { get; set; } = null!;

    public string Estado { get; set; } = null!;

    public int IdClub { get; set; }

    public int IdCategoria { get; set; }

    public virtual Categoria IdCategoriaNavigation { get; set; } = null!;

    public virtual Clubfutbol IdClubNavigation { get; set; } = null!;

    //public virtual ICollection<Pasejugador> Pasejugadors { get; set; } = new List<Pasejugador>();
}
