using TicketToCode.Client.Components;
using TicketToCode.Client.Services; // För att kunna injicera EventService
using Microsoft.Extensions.Configuration; // För att läsa appsettings.json
using Microsoft.AspNetCore.Components.Server; // För server-side Blazor

var builder = WebApplication.CreateBuilder(args);

// Lägg till HttpClient för EventService och sätt bas-URL:en för API:t
builder.Services.AddHttpClient<EventService>(client =>
{
    var apiUrl = builder.Configuration["ApiUrl"]; // Läser från appsettings.json
    client.BaseAddress = new Uri(apiUrl); // Sätter base address för HttpClient
});

// Lägg till Blazor Server-tjänster
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(); // För server-side Blazor

// Lägg till EventService för injektion i komponenter
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

// Lägg till Blazor komponenter
app.MapRazorComponents<App>();

app.Run();
