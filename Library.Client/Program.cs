using Library.Client.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddScoped<HttpClient>(x => new HttpClient
{
    BaseAddress = new Uri( builder.Configuration.GetConnectionString("ApiAddress") ?? "")
});
builder.Services.AddScoped<HttpClientService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();

var app = builder.Build();


app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
