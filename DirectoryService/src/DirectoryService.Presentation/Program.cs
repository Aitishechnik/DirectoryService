using System.Text.Json.Serialization;
using DirectoryService.Application;
using DirectoryService.Domain.Shared;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.DbContexts;
using DirectoryService.Presentation.Response;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    c.SchemaFilter<EnumSchemaFilter>();

    c.MapType<Envelope<Errors>>(() => new OpenApiSchema
    {
        Type = "object",
        Properties =
        {
            ["errors"] = new OpenApiSchema
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.Schema,
                    Id = "Error",
                },
            },
        },
    });
});

builder.Services.AddScoped(_ =>
    new DirectoryServiceDbContext(
        builder.Configuration.GetConnectionString("Database")!));

builder.Services
    .AddApplication()
    .AddInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

app.Run();