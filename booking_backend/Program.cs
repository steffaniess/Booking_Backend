using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using booking_backend.Data;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);

// Läs in anslutningssträngen för MongoDB
string mongoConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");

// Lägg till MongoDB-klienten i Dependency Injection-containern
builder.Services.AddSingleton<IMongoClient>(serviceProvider =>
{
    return new MongoClient(mongoConnectionString);
});

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHttpsRedirection();

// Tillåt anrop från frontend på localhost:3000
app.UseCors(builder =>
{
    builder.WithOrigins("http://localhost:3000")
           .AllowAnyHeader()
           .AllowAnyMethod();
});

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
    });
}

app.Run();
