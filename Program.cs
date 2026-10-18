using LigaDeJugadoresAPI.Data;
using Microsoft.EntityFrameworkCore;
using LigaDeJugadoresAPI.Interfaces;
using LigaDeJugadoresAPI.Services;

var builder = WebApplication.CreateBuilder(args);

// Registrar los controladores
builder.Services.AddControllers();

// configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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

app.UseHttpsRedirection();

// Habilitar las rutas de los controladores
app.MapControllers();


app.Run();


