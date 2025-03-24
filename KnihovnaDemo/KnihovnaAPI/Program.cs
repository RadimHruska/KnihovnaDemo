using KnihovnaAPI.Data;
using KnihovnaAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Přidání služeb do kontejneru
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Knihovna API", Version = "v1" });
});

// Konfigurace připojení k databázi - získá connection string z konfigurace
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Console.WriteLine($"Použitý connection string: {connectionString}");

builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlServer(connectionString));

// Registrace repozitářů
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<ILendRepository, LendRepository>();

// Povolení CORS pro klienta - konfigurace pro více původů
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowKnihovnaClient",
        policy => policy
            .WithOrigins(
                "http://localhost:5173",  // Vývojový server
                "http://localhost:5174",
                "http://knihovna-client:80" // Kontejnerový klient
            )
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials());
});

var app = builder.Build();

// Konfigurace HTTP request pipeline
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Knihovna API v1");
    c.RoutePrefix = string.Empty; // Nastaví Swagger UI jako root stránku
});

// Vypíšeme, kde aplikace poslouchá pro účely ladění
Console.WriteLine($"Aplikace je nakonfigurována na adrese: {(app.Urls.Count > 0 ? string.Join(", ", app.Urls) : "není explicitně nastaveno")}");

// Nová konfigurace pro .NET 8
app.UseHttpsRedirection();
app.UseCors("AllowKnihovnaClient");
app.UseAuthorization();

// Mapování endpointů
app.MapControllers();

// Inicializace databáze při spuštění aplikace
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<LibraryContext>();
        Console.WriteLine("Probíhá inicializace databáze...");
        DbInitializer.Initialize(context);
        Console.WriteLine("Databáze byla úspěšně inicializována.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Došlo k chybě při inicializaci databáze: {ex.Message}");
    }
}

app.Run(); 