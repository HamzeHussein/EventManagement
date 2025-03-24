using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.OpenApi.Models;
using TicketToCode.Api.Services;
using TicketToCode.Core.Data;
using TicketToCode.Core.Models;
using TicketToCode.Client.Services;  // Lägg till denna rad för att importera EventService

var builder = WebApplication.CreateBuilder(args);

// Hämta connection string och konfigurera DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EventManagementDbContext>(options =>
    options.UseSqlServer(connectionString));

// Lägg till Swagger för att dokumentera API:t
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TicketToCode API", Version = "v1" });
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "TicketToCode.Api.xml")); // För att inkludera XML-kommentarer om du har det
});

// Lägg till services för databasen och autentisering
builder.Services.AddSingleton<IDatabase, Database>();
builder.Services.AddScoped<IAuthService, AuthService>();

// Lägg till EventService för frontend (Blazor-klienten)
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
        options.DefaultModelsExpandDepth(-1);  // Döljer modelbeskrivningarna om du vill
    });
}

// Middleware för HTTPS, autentisering och auktorisation
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

// Mappa controllers (för controller-baserade API)
app.MapControllers();  // Byt till detta om du använder controllers

// Kör applikationen
app.Run();
