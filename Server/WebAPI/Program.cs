using FileRepositories;
using RepositoryContracts;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EfcRepositories;

var builder = WebApplication.CreateBuilder(args);

// Add controllers
builder.Services.AddControllers();

// Register repositories
builder.Services.AddScoped<IPostRepository, EfcPostRepository>();
builder.Services.AddScoped<ICommentRepository, EfcCommentRepository>();
builder.Services.AddScoped<IUserRepository, EfcUserRepository>();
builder.Services.AddDbContext<EfcRepositories.AppContext>();

var app = builder.Build();

// Optional: use HTTPS redirection
app.UseHttpsRedirection();

// Map controller endpoints
app.MapControllers();

// Run the application
app.Run();
