using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyCinemaDiary.API.Controllers;
using MyCinemaDiary.Application;
using MyCinemaDiary.Infrastructure.Data;
using MyCinemaDiary.Infrastructure.ExternalApiClients;
using MyCinemaDiary.Infrastructure.Repositories;
using MyCinemaDiary.Infrastructure.Services;
using System.Text.Json;
using System.IdentityModel.Tokens.Jwt;

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
builder.Services.AddScoped<IDiaryEntriesRepository, DiaryEntriesRepository>();
builder.Services.AddScoped<DiaryEntries>();
builder.Services.AddScoped<DiaryEntriesController>();

// Configuring JWT authentication
var key = Encoding.UTF8.GetBytes(getJWTKey());
builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false; // Set to true in production
    x.SaveToken = true;
    x.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false, // Not validating the issuer
        ValidateAudience = false, // Not validating the audience
        ValidateLifetime = true, // Validate the token expiration
        ValidateIssuerSigningKey = true, // Validate the signing key
        IssuerSigningKey = new SymmetricSecurityKey(key) // Your signing key
    };
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// Enables static file serving:
app.UseHttpsRedirection();
app.UseStaticFiles();
JwtSecurityTokenHandler.DefaultMapInboundClaims = false; // Prevets sub to be mapped to http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier
JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

string getJWTKey()
{
    string solutionDirectory = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\..\.."));

    string filePath = Path.Combine(solutionDirectory, "MyCinemaDiary", "secrets.json");
    StreamReader reader = new(filePath);
    var text = reader.ReadToEnd();

    return JsonDocument.Parse(text).RootElement.GetProperty("Jwt").GetProperty("Key").ToString();
}