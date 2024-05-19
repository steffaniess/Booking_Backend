using booking_backend.Helpers;
using booking_backend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddEnvironmentVariables();

var mongoDbConnectionString = builder.Configuration.GetConnectionString("MongoDBConnection");
if (string.IsNullOrEmpty(mongoDbConnectionString))
{
    Console.WriteLine("MongoDB connection string is not configured.");
    throw new Exception("MongoDB connection string is not configured.");
}
else
{
    Console.WriteLine($"MongoDB connection string: {mongoDbConnectionString}");
}


//Services to the container
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
builder.Services.AddSingleton<IEmailService, EmailService>();
builder.Services.AddSingleton<MongoDBContext>();
builder.Services.AddControllers();

//Swagger generation with XML comments
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    //Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath)) //Ensure the XML file exists
    {
        c.IncludeXmlComments(xmlPath);
    }
});

//Add CORS configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", //Giving the CORS policy a specific name
        builder => builder.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
});

var app = builder.Build();

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}

app.UseHttpsRedirection();
app.UseCors("AllowSpecificOrigin"); //Applying CORS policy to the application
app.UseAuthorization();
app.MapControllers();

app.Run();
