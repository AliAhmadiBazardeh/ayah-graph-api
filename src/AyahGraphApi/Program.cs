using AyahGraphApi.Application.Services;
using AyahGraphApi.Infrastructure;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(
            new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc(
        "v1",
        new OpenApiInfo
        {
            Title = "Ayah Graph API",
            Version = "v1"
        });
});
builder.Services.AddScoped<
    IVerseRelationService,
    VerseRelationService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapSwagger("/openapi/{documentName}.json");

    app.MapScalarApiReference(options =>
    {
        options.WithTitle("Ayah Graph API");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();