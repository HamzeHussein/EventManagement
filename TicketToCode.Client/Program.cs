using TicketToCode.Client.Components;
using TicketToCode.Client.Services; // Lägg till för att kunna injicera EventService

var builder = WebApplication.CreateBuilder(args);

// Lägg till HttpClient för EventService
builder.Services.AddHttpClient<EventService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/"); // Lägg till din API-URL här
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Lägg till EventService för injektion i komponenter
builder.Services.AddScoped<EventService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
