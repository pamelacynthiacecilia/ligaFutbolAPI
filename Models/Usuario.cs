using System;
using System.Collections.Generic;

namespace LigaDeJugadoresAPI.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Rol { get; set; } = null!;

    public bool Activo { get; set; }

    public virtual Comisiondirectiva? Comisiondirectiva { get; set; }
}
