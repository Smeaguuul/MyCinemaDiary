using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.API.Controllers;
using MyCinemaDiary.Application;
using MyCinemaDiary.Infrastructure.Data;
using MyCinemaDiary.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDBContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration["DbConnectionString"]!);
});

builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<Movies>();
builder.Services.AddScoped<MoviesController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
