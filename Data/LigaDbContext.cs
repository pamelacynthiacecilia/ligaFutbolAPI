using System;
using System.Collections.Generic;
using LigaDeJugadoresAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LigaDeJugadoresAPI.Data;

public partial class LigaDbContext : DbContext
{
    public LigaDbContext(DbContextOptions<LigaDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categoria> Categoria { get; set; }

    public virtual DbSet<Clubfutbol> Clubfutbols { get; set; }

    public virtual DbSet<Comisiondirectiva> Comisiondirectivas { get; set; }

    public virtual DbSet<Jugador> Jugadors { get; set; }

    //public virtual DbSet<Pasejugador> Pasejugadors { get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("categoria");
 
            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.EdadMax)
                .HasColumnType("int(11)")
                .HasColumnName("edadMax");
            entity.Property(e => e.EdadMin)
                .HasColumnType("int(11)")
                .HasColumnName("edadMin");
            entity.Property(e => e.Nombre)
                .HasMaxLength(50)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Clubfutbol>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("clubfutbol");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Barrio)
                .HasMaxLength(255)
                .HasColumnName("barrio");
            entity.Property(e => e.Escudo)
                .HasMaxLength(255)
                .HasColumnName("escudo");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.FechaFundacion).HasColumnName("fechaFundacion");
            entity.Property(e => e.Localidad)
                .HasMaxLength(100)
                .HasColumnName("localidad");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Comisiondirectiva>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("comisiondirectiva");

            entity.HasIndex(e => e.IdClub, "idClub");

            entity.HasIndex(e => e.IdUsuario, "idUsuario").IsUnique();

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .HasColumnName("dni");
            entity.Property(e => e.IdClub)
                .HasColumnType("int(11)")
                .HasColumnName("idClub");
            entity.Property(e => e.IdUsuario)
                .HasColumnType("int(11)")
                .HasColumnName("idUsuario");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Puesto)
                .HasMaxLength(50)
                .HasColumnName("puesto");
            entity.Property(e => e.Telefono)
                .HasMaxLength(20)
                .HasColumnName("telefono");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Comisiondirectivas)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("comisiondirectiva_ibfk_1");

            entity.HasOne(d => d.IdUsuarioNavigation).WithOne(p => p.Comisiondirectiva)
                .HasForeignKey<Comisiondirectiva>(d => d.IdUsuario)
                .HasConstraintName("comisiondirectiva_ibfk_2");
        });

        modelBuilder.Entity<Jugador>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("jugador");

            entity.HasIndex(e => e.Dni, "dni").IsUnique();

            entity.HasIndex(e => e.IdCategoria, "idCategoria");

            entity.HasIndex(e => e.IdClub, "idClub");

            entity.Property(e => e.Id)
                .HasColumnType("int(11)")
                .HasColumnName("id");
            entity.Property(e => e.Apellido)
                .HasMaxLength(100)
                .HasColumnName("apellido");
            entity.Property(e => e.DispPase)
                .HasMaxLength(10)
                .HasColumnName("dispPase");
            entity.Property(e => e.Dni)
                .HasMaxLength(20)
                .HasColumnName("dni");
            entity.Property(e => e.Estado)
                .HasMaxLength(20)
                .HasColumnName("estado");
            entity.Property(e => e.FechaNac).HasColumnName("fechaNac");
            entity.Property(e => e.IdCategoria)
                .HasColumnType("int(11)")
                .HasColumnName("idCategoria");
            entity.Property(e => e.IdClub)
                .HasColumnType("int(11)")
                .HasColumnName("idClub");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");

            entity.HasOne(d => d.IdCategoriaNavigation).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.IdCategoria)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jugador_ibfk_2");

            entity.HasOne(d => d.IdClubNavigation).WithMany(p => p.Jugadors)
                .HasForeignKey(d => d.IdClub)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("jugador_ibfk_1");
        });

        modelBuilder.Entity<Usuario>(entity =>
{
    entity.HasKey(e => e.Id).HasName("PRIMARY");

    entity.ToTable("usuario");

    entity.HasIndex(e => e.Email, "email").IsUnique();

    entity.Property(e => e.Id)
        .HasColumnType("int(11)")
        .HasColumnName("id");

    entity.Property(e => e.Nombre)
      .HasMaxLength(30)
        .HasColumnName("nombre");

    entity.Property(e => e.Apellido)
      .HasMaxLength(50)
        .HasColumnName("apellido");

    entity.Property(e => e.Activo)
        .HasColumnName("activo");

    entity.Property(e => e.Email)
        .HasColumnName("email");

    entity.Property(e => e.Password)
        .HasMaxLength(255)
        .HasColumnName("password");

    entity.Property(e => e.Rol)
        .HasMaxLength(50)
        .HasColumnName("rol");
});

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
