using TicketToCode.Client.Components;
using TicketToCode.Client.Services; // För att kunna injicera EventService
using Microsoft.Extensions.Configuration; // För att läsa appsettings.json
using Microsoft.AspNetCore.Components; // För server-side Blazor
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args); // För server-side Blazor

// Lägg till HttpClient för EventService och sätt bas-URL:en för API:t
builder.Services.AddHttpClient<EventService>(client =>
{
    // Hämta API URL från appsettings.json
    var apiUrl = builder.Configuration["ApiUrl"]; // Läser från appsettings.json
    client.BaseAddress = new Uri(apiUrl); // Sätter base address för HttpClient
});

// Lägg till Razor Components för server-side Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // Lägg till för Blazor

// Lägg till EventService för injektion i komponenter
builder.Services.AddScoped<EventService>(); // Lägg till EventService för DI

var app = builder.Build();

// Konfigurera appen
app.Run();

