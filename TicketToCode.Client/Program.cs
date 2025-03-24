using TicketToCode.Client.Components;
using TicketToCode.Client.Services; // F�r att kunna injicera EventService
using Microsoft.Extensions.Configuration; // F�r att l�sa appsettings.json
using Microsoft.AspNetCore.Components; // F�r server-side Blazor
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args); // F�r server-side Blazor

// L�gg till HttpClient f�r EventService och s�tt bas-URL:en f�r API:t
builder.Services.AddHttpClient<EventService>(client =>
{
    // H�mta API URL fr�n appsettings.json
    var apiUrl = builder.Configuration["ApiUrl"]; // L�ser fr�n appsettings.json
    client.BaseAddress = new Uri(apiUrl); // S�tter base address f�r HttpClient
});

// L�gg till Razor Components f�r server-side Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // L�gg till f�r Blazor

// L�gg till EventService f�r injektion i komponenter
builder.Services.AddScoped<EventService>(); // L�gg till EventService f�r DI

var app = builder.Build();

// Konfigurera appen
app.Run();

