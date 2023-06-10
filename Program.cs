using task6x.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Connections;
using task6x.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();

builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

builder.Services.AddHttpContextAccessor();
builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.MapHub<HangmanHub>("/Hangman");
app.MapHub<CrocodileHub>("/Crocodile");
app.MapHub<GameSessionHub>("/GameSession");

app.UseSession();

app.Run();
