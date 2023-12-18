using API.Extensions;
using API.Middleware;
using Core.Models.Identity;
using Infrastructure.Data;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AppServices(builder.Configuration);
builder.Services.IdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

app.UseStatusCodePagesWithReExecute("/errors/{0}");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AngularAppPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.UseStaticFiles();   //return static files

app.MapControllers();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var context = services.GetRequiredService<AppDbContext>();
var identityContext = services.GetRequiredService<AppIdentityDbContext>();
var userManeger = services.GetRequiredService<UserManager<AppUser>>();
var logeer = services.GetRequiredService<ILogger<Program>>();
try
{
    await context.Database.MigrateAsync();
    await identityContext.Database.MigrateAsync();
    await AppDbContextSeed.SeedAsync(context);
    await IdentitySeed.SeedUsersAsync(userManeger);
}
catch (Exception ex)
{
    logeer.LogError(ex.Message, "An error occured during migration");
}

app.Run();
