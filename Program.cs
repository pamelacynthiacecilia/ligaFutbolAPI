using LigaDeJugadoresAPI.Data;
using Microsoft.EntityFrameworkCore;
using LigaDeJugadoresAPI.Interfaces;
using LigaDeJugadoresAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;


var builder = WebApplication.CreateBuilder(args);

// Registrar los controladores
builder.Services.AddControllers();

// configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Definir autenticación JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresá el token JWT obtenido en el login."
    });

    // Indicar a Swagger que puede utilizar JWT
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


// Obtener la cadena de conexion desde appsettings.json
var connectionString = builder.Configuration
    .GetConnectionString("LigaDB")
    ?? throw new InvalidOperationException(
        "No se encontro la cadena de conexion LigaDB."
    );

// Registrar el contexto de Entity Framework Core
builder.Services.AddDbContext<LigaDbContext>(options =>
    options.UseMySql(
        connectionString,
        new MariaDbServerVersion(new Version(10, 4, 32))
    )
);
 
// Registrar los servicios de la API
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IClubesService, ClubesService>();
builder.Services.AddAuthentication(
    JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    builder.Configuration["Jwt:Key"]
                    ?? throw new InvalidOperationException(
                        "No se configuró Jwt:Key.")))
        };
    });

// Registrar los servicios de autorización
builder.Services.AddAuthorization();


var app = builder.Build();



// Configure the HTTP request pipeline. Swagger y prueba de conexion en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

 // Endpoint temporal para verificar la conexion a MySQL
    app.MapGet("/test-db", async (LigaDbContext db) =>
    {
        bool conectado = await db.Database.CanConnectAsync();

        return conectado
            ? Results.Ok("Conexion a MySQL exitosa")
            : Results.Problem("No se pudo conectar a MySQL");
    });


}

app.UseStaticFiles();
app.UseHttpsRedirection();

// Validar la identidad del usuario mediante JWT, Comprueba la firma, vigencia, emisor y audiencia del token
app.UseAuthentication();

// Verificar si el usuario tiene permisos exigidos por el endpoint 
app.UseAuthorization();

// Habilitar las rutas de los controladores
app.MapControllers();


app.Run();


