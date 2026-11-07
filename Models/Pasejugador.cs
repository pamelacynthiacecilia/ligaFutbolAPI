using System;
using System.Collections.Generic;

namespace LigaDeJugadoresAPI.Models;
//No entra en el alcance del proyecto final el historial de pase de jugadores
public partial class Pasejugador
{
    public int Id { get; set; }

    public int IdJugador { get; set; }

    public int IdClubOrigen { get; set; }

    public int IdClubDestino { get; set; }

    public DateOnly FechaPase { get; set; }

    public DateOnly? FechaFinPase { get; set; }

    public string Estado { get; set; } = null!;

    public virtual Clubfutbol IdClubDestinoNavigation { get; set; } = null!;

    public virtual Clubfutbol IdClubOrigenNavigation { get; set; } = null!;

    public virtual Jugador IdJugadorNavigation { get; set; } = null!;
}
