using System;
using System.Collections.Generic;

namespace LigaDeJugadoresAPI.Models;

public partial class Clubfutbol
{
    public int Id { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Escudo { get; set; }

    public string? Barrio { get; set; }

    public string? Localidad { get; set; }

    public DateOnly? FechaFundacion { get; set; }

    public string Estado { get; set; } = null!;

    public virtual ICollection<Comisiondirectiva> Comisiondirectivas { get; set; } = new List<Comisiondirectiva>();

    public virtual ICollection<Jugador> Jugadors { get; set; } = new List<Jugador>();

    //public virtual ICollection<Pasejugador> PasejugadorIdClubDestinoNavigations { get; set; } = new List<Pasejugador>();

    //public virtual ICollection<Pasejugador> PasejugadorIdClubOrigenNavigations { get; set; } = new List<Pasejugador>();
}
