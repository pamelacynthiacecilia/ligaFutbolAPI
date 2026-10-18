using System;
using System.Collections.Generic;

namespace LigaDeJugadoresAPI.Models;

public partial class Comisiondirectiva
{
    public int Id { get; set; }

    public string Puesto { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Apellido { get; set; } = null!;

    public string? Telefono { get; set; }

    public string? Dni { get; set; }

    public int IdClub { get; set; }

    public int? IdUsuario { get; set; }

    public virtual Clubfutbol IdClubNavigation { get; set; } = null!;

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
