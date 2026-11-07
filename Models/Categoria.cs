using System;
using System.Collections.Generic;

namespace LigaDeJugadoresAPI.Models;

public partial class Categoria{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public int EdadMin { get; set; }

    public int EdadMax { get; set; }

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();
}
