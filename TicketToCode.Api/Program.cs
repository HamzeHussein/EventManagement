using Microsoft.EntityFrameworkCore;
using TicketToCode.Api.Endpoints;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;
using TicketToCode.Client.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using System.Net.Http; // Lägg till denna rad

var builder = WebApplication.CreateBuilder(args);

// Hämta connection string och konfigurera DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EventManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

// Lägg till OpenAPI och Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketToCode API", Version = "v1" });
    // Ta bort raden som refererar till XML-kommentarer om du inte har den
    // options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TicketToCode.Api.xml"));
});

// Lägg till services för databasen och autentisering
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Lägg till EventService för frontend (Blazor-klienten) och HttpClient
builder.Services.AddHttpClient<EventService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001"); // Uppdatera med din API:s bas-URL
});
builder.Services.AddScoped<EventService>();

// Lägg till autentisering via Cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "auth";
        options.Cookie.HttpOnly = true;
        options.Cookie.SameSite = SameSiteMode.Strict;
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Konfigurera Swagger för utvecklingsmiljö
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Lägg till denna rad
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "TicketToCode API v1");
        options.DefaultModelsExpandDepth(-1);
    });
}

// Middleware för HTTPS, autentisering och auktorisation
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Mappla alla endpoints
app.MapEndpoints<Program>();

// Kör applikationen
app.Run();
