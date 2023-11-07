using Amazon.Auth.AccessControlPolicy;
using Library.Common.Exceptions;
using Library.Common.Options;
using Library.Di;
using Library.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new Microsoft.OpenApi.Models.OpenApiInfo
        {
            Title = "My API - V1",
            Version = "v1"
        }
     );

    var filePath = Path.Combine(System.AppContext.BaseDirectory, "MyApi.xml");

    if (!File.Exists(filePath))
    {
        File.Create(filePath);
    }

    c.IncludeXmlComments(filePath);
});

builder.Services.AddCors(opt =>
{
    opt.AddDefaultPolicy(pol =>
    {
        pol.AllowAnyHeader();
        pol.AllowAnyMethod();
        pol.AllowAnyOrigin();
        pol.SetIsOriginAllowed(origin => true);
        pol.AllowCredentials();
    });
});


builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetSection("Connection"));
builder.Services.Configure<RedisOptions>(builder.Configuration.GetSection("Redis"));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));


builder.Services.AddStackExchangeRedisCache(conf =>
{

    var conString = builder.Configuration.GetSection("Connection:RedisConnection").Value;
    Console.WriteLine(conString);
    conf.Configuration = conString;
});

bool isUseAuth = (bool)builder.Configuration.GetSection("Connection").GetValue<bool>("UseAuth");

if (isUseAuth)
{
    builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
    builder.Services.AddAuthorization(options =>
    {
        options.AddPolicy("Default", policy =>
        {
            policy.RequireAuthenticatedUser();
        });
        options.AddPolicy("ADMIN", policy =>
        {
            policy.RequireAuthenticatedUser();
            policy.RequireRole("ADMIN");
        });
    });

}


var serilogConfig = builder.Configuration.GetSection("Serilog");

builder.Host.UseSerilog((context, config) =>
    config
    .ReadFrom.Configuration(context.Configuration)
    .WriteTo.Console());


Di.AddServices(builder.Services);

var app = builder.Build();

app.UseStaticFiles();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (isUseAuth)
{
    app.UseAuthorization();
}

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
