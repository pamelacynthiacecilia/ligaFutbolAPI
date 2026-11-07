using LigaDeJugadoresAPI.Data;
using LigaDeJugadoresAPI.DTOs.Usuarios;
using LigaDeJugadoresAPI.Interfaces;
using LigaDeJugadoresAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace LigaDeJugadoresAPI.Services;

public class UsuarioService : IUsuarioService
{
    private readonly LigaDbContext _context;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public UsuarioService(LigaDbContext context)
    {
        _context = context;
    }
    public async Task<bool> ExistenUsuariosAsync()
{
    return await _context.Usuarios.AnyAsync();
}
    public async Task<UsuarioDto> CrearUsuarioAsync(CrearUsuarioDto dto)
    {
        // 1. Normalizar el email
        var email = dto.Email.Trim().ToLowerInvariant();

        // 2. Verificar si ya existe un usuario con ese email
        var existeUsuario = await _context.Usuarios
            .AnyAsync(u => u.Email == email);

        if (existeUsuario)
        {
            throw new InvalidOperationException(
                "Ya existe un usuario registrado con ese email.");
        }

        // 3. Validar el rol
        var rolesPermitidos = new[] { "Admin", "Presidente", "Secretario" };

        if (!rolesPermitidos.Contains(dto.Rol))
        {
            throw new ArgumentException(
                "El rol ingresado no es válido.");
        }

        // 4. Crear la entidad Usuario
        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Apellido = dto.Apellido.Trim(),
            Email = email,
            Rol = dto.Rol,
            Activo = true
        };

        // 5. Generar el hash de la contraseña
        usuario.Password = _passwordHasher.HashPassword(
            usuario,
            dto.Password
        );

        // 6. Guardar el usuario en MySQL
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();

        // 7. Devolver el DTO sin la contraseña
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            Apellido = usuario.Apellido,
            Email = usuario.Email,
            Rol = usuario.Rol,
            Activo = usuario.Activo
        };
    }

    public async Task<bool> CambiarPasswordAsync(
    int idUsuario,
    CambiarPasswordDto dto)
{
    // 1. Buscar el usuario
    var usuario = await _context.Usuarios
        .FirstOrDefaultAsync(u => u.Id == idUsuario && u.Activo);

    if (usuario == null)
    {
        throw new KeyNotFoundException(
            "El usuario no existe o está inactivo.");
    }

    // 2. Verificar la contraseña actual
    var resultado = _passwordHasher.VerifyHashedPassword(
        usuario,
        usuario.Password,
        dto.PasswordActual
    );

    if (resultado == PasswordVerificationResult.Failed)
    {
        throw new ArgumentException(
            "La contraseña actual es incorrecta.");
    }

    // 3. Verificar que la nueva contraseña sea diferente
    var resultadoNueva = _passwordHasher.VerifyHashedPassword(
        usuario,
        usuario.Password,
        dto.PasswordNueva
    );

    if (resultadoNueva != PasswordVerificationResult.Failed)
    {
        throw new ArgumentException(
            "La nueva contraseña debe ser diferente de la actual.");
    }

    // 4. Generar el hash de la nueva contraseña
    usuario.Password = _passwordHasher.HashPassword(
        usuario,
        dto.PasswordNueva
    );

    // 5. Guardar los cambios
    await _context.SaveChangesAsync();

    return true;
}

public async Task<bool> RestablecerPasswordAsync(
    int idUsuario,
    RestablecerPasswordDto dto)
{
    // 1. Buscar el usuario
    var usuario = await _context.Usuarios
        .FirstOrDefaultAsync(u => u.Id == idUsuario && u.Activo);

    if (usuario == null)
    {
        throw new KeyNotFoundException(
            "El usuario no existe o está inactivo.");
    }

    // 2. Generar el hash de la nueva contraseña
    usuario.Password = _passwordHasher.HashPassword(
        usuario,
        dto.PasswordNueva
    );

    // 3. Guardar los cambios
    await _context.SaveChangesAsync();

    return true;
}
}