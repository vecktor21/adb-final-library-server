using Library.Client.Models;
using Library.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddBlazorBootstrap();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<HttpClient>(x => new HttpClient
{
    BaseAddress = new Uri( builder.Configuration.GetConnectionString("ApiAddress") ?? "")
});

builder.Services.Configure<ConnectionOptions>(builder.Configuration.GetSection("ConnectionStrings"));
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<CartService>();

var app = builder.Build();


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
