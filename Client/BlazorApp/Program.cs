using BlazorApp.Auth;
using BlazorApp.Components;
using BlazorApp.Services;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://localhost:7093/")
});

// Register HTTP-backed client services
builder.Services.AddScoped<ICommentService, HttpCommentService>();
builder.Services.AddScoped<IPostService, HttpPostService>();
builder.Services.AddScoped<IUserService, HttpUserService>();

// 🔥 Add authentication + authorization
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies");

builder.Services.AddAuthorization();

// Register your SimpleAuthProvider
builder.Services.AddScoped<AuthenticationStateProvider, SimpleAuthProvider>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForErrors: true);

app.UseHttpsRedirection();

// 🔥 Add authentication & authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
