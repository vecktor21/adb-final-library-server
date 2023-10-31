using Library.Common.Exceptions;
using Library.Dal.Options;
using Library.Di;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssemblies() );
});

var serilogConfig = builder.Configuration.GetSection("Serilog");

builder.Host.UseSerilog((context, config) =>
    config
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console());



builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetSection("Connection"));

Di.AddServices(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler(applicationBuilder =>
{
    applicationBuilder.Run(async context =>
    {
        var handlerFeature = context.Features.Get<IExceptionHandlerFeature>();
        if (handlerFeature?.Error is ResponseResultException exception)
        {
            context.Response.ContentType = "application/json";

            var status = exception.Code;

            context.Response.StatusCode = (int)status;

            await context.Response.WriteAsJsonAsync(new
            {
                Code = exception.Code,
                Message = exception.Message,
                Field = exception.Field
            });
        }
        else
        {
            throw handlerFeature?.Error;
        }
    });
});

app.Run();
