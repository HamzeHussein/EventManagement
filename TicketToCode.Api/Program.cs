using Microsoft.EntityFrameworkCore;
using TicketToCode.Api.Endpoints;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;
using TicketToCode.Client.Services; // Lägg till denna rad för att importera EventService

var builder = WebApplication.CreateBuilder(args);

// Hämta connection string och konfigurera DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EventManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

// Lägg till OpenAPI och Swagger
builder.Services.AddOpenApi();

// Lägg till services för databasen och autentisering
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Lägg till EventService för frontend (Blazor-klienten)
builder.Services.AddScoped<EventService>(); // Lägg till denna rad för att registrera EventService

// Lägg till autentisering via Cookies
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Konfigurera utvecklingsspecifika inställningar för Swagger
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "v1");
        options.DefaultModelsExpandDepth(-1);
    });
}

app.UseHttpsRedirection(); // Använd HTTPS om applikationen är på HTTPS
app.UseAuthentication(); // Lägg till autentisering för att skydda API:t
app.UseAuthorization();  // Lägg till auktorisation

// Mappla alla endpoints
app.MapEndpoints<Program>();

// Kör applikationen
app.Run();
