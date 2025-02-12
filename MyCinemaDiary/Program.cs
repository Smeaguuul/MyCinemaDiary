using Microsoft.EntityFrameworkCore;
using MyCinemaDiary.API.Controllers;
using MyCinemaDiary.Application;
using MyCinemaDiary.Infrastructure.Data;
using MyCinemaDiary.Infrastructure.ExternalApiClients;
using MyCinemaDiary.Infrastructure.Repositories;
using MyCinemaDiary.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AppDBContext>(optionsBuilder =>
{
    optionsBuilder.UseNpgsql(builder.Configuration["DbConnectionString"]!);
});

builder.Services.AddSingleton<HttpClientService>();
builder.Services.AddScoped<TheTvDbAPI>();
builder.Services.AddScoped<IMoviesRepository, MoviesRepository>();
builder.Services.AddScoped<Movies>();
builder.Services.AddScoped<MoviesController>();
builder.Services.AddScoped<MovieSearchController>();
builder.Services.AddScoped<IUsersRepository, UsersRepository>();
builder.Services.AddScoped<Users>();
builder.Services.AddScoped<UsersController>();
builder.Services.AddScoped<IDiaryEntriesRepository, DiaryEntriesRepository>();
builder.Services.AddScoped<DiaryEntries>();
builder.Services.AddScoped<DiaryEntriesController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enables static file serving:
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseAuthorization();

app.MapControllers();

app.Run();
