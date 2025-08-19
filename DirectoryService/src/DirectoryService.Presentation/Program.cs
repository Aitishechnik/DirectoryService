using System.Text.Json.Serialization;
using DirectoryService.Application;
using DirectoryService.Domain.Shared;
using DirectoryService.Infrastructure;
using DirectoryService.Infrastructure.DbContexts;
using DirectoryService.Presentation.Middleware;
using DirectoryService.Presentation.Response;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Debug()
    .WriteTo.Seq(builder.Configuration.GetConnectionString("Seq")
                 ?? throw new ArgumentNullException("No Seq in ConnectionStrings"))
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Rounting", LogEventLevel.Warning)
    .CreateLogger();

builder.Services.AddSerilog();

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

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionMiddleware();

app.MapControllers();

app.Run();