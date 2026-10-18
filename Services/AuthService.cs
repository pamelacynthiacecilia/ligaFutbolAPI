using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LigaDeJugadoresAPI.Data;
using LigaDeJugadoresAPI.DTOs.Auth;
using LigaDeJugadoresAPI.Interfaces;
using LigaDeJugadoresAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace LigaDeJugadoresAPI.Services;
//Consultar a la tabla usuario mediante LigaDbContext
//Verificar contraseña ingresada contra hash almacenado
//Generar token JWT si el usuario existe, esta activo y sus credenciales son validas


public class AuthService : IAuthService
{
    private readonly LigaDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly PasswordHasher<Usuario> _passwordHasher = new();

    public AuthService(
        LigaDbContext context,
        IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }


public async Task<LoginResponse?> LoginAsync(LoginDto loginDto)
{
    var usuario = await _context.Usuarios
        .AsNoTracking()
        .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

    if (usuario == null || !usuario.Activo)
        return null;

    var resultado = _passwordHasher.VerifyHashedPassword(
        usuario,
        usuario.Password,
        loginDto.Password
    );

    if (resultado == PasswordVerificationResult.Failed)
        return null;

    var token = GenerarToken(usuario);

    return new LoginResponse
    {
        Token = token,
        Email = usuario.Email,
        Rol = usuario.Rol
    };
}

private string GenerarToken(Usuario usuario)
{
    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
        new Claim(ClaimTypes.Email, usuario.Email),
        new Claim(ClaimTypes.Role, usuario.Rol)
    };

    var clave = _configuration["Jwt:Key"]
        ?? throw new InvalidOperationException(
            "No se configuró Jwt:Key.");

    var signingKey = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(clave));

    var credenciales = new SigningCredentials(
        signingKey,
        SecurityAlgorithms.HmacSha256);

    var expiracionMinutos = int.Parse(
        _configuration["Jwt:ExpirationMinutes"] ?? "60");

    var token = new JwtSecurityToken(
        issuer: _configuration["Jwt:Issuer"],
        audience: _configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(expiracionMinutos),
        signingCredentials: credenciales
    );

    return new JwtSecurityTokenHandler().WriteToken(token);
}

}
   
    {
        var usuario = await _context.Usuarios
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == loginDto.Email);

        if (usuario == null || !usuario.Activo)
            return null;

        var resultado = _passwordHasher.VerifyHashedPassword(
            usuario,
            usuario.Password,
            loginDto.Password
        );

        if (resultado == PasswordVerificationResult.Failed)
            return null;

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var clave = _configuration["Jwt:Key"]
            ?? throw new InvalidOperationException(
                "No se configuró Jwt:Key.");

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(clave));

        var credenciales = new SigningCredentials(
            signingKey,
            SecurityAlgorithms.HmacSha256);

        var expiracionMinutos = int.Parse(
            _configuration["Jwt:ExpirationMinutes"] ?? "60");

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expiracionMinutos),
            signingCredentials: credenciales
        );

        return new LoginResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Email = usuario.Email,
            Rol = usuario.Rol
        };
    }
}