using TicketToCode.Client.Components;
using TicketToCode.Client.Services; // L�gg till f�r att kunna injicera EventService

var builder = WebApplication.CreateBuilder(args);

// L�gg till HttpClient f�r EventService
builder.Services.AddHttpClient<EventService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:5001/"); // L�gg till din API-URL h�r
});

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// L�gg till EventService f�r injektion i komponenter
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
