using TicketToCode.Client.Components;
using TicketToCode.Client.Services; // F�r att kunna injicera EventService
using Microsoft.Extensions.Configuration; // F�r att l�sa appsettings.json
using Microsoft.AspNetCore.Components.Server; // F�r server-side Blazor

var builder = WebApplication.CreateBuilder(args);

// L�gg till HttpClient f�r EventService och s�tt bas-URL:en f�r API:t
builder.Services.AddHttpClient<EventService>(client =>
{
    var apiUrl = builder.Configuration["ApiUrl"]; // L�ser fr�n appsettings.json
    client.BaseAddress = new Uri(apiUrl); // S�tter base address f�r HttpClient
});

// L�gg till Blazor Server-tj�nster
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // F�r server-side Blazor

// L�gg till EventService f�r injektion i komponenter
builder.Services.AddScoped<EventService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAntiforgery();

// L�gg till Blazor komponenter
app.MapRazorComponents<App>();

app.Run();
