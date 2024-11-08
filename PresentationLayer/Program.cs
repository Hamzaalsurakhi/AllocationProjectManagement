using BusinessLayer.Interfaces;
using BusinessLayer.Services;
using BusinessLayer.SignalR;
using DataLayer.Data;
using DataLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PresentationLayer.ExtensionsServices;




var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddServiceExtensions(builder.Configuration);
builder.Services.AddIdentityServiceExtensions(builder.Configuration);

builder.Services.AddSignalR();

builder.Services.AddScoped<INotificationService, NotificationService>();

// AutoMapper Configuration
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddDbContext<AppDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("ConnectionString"));


});
var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
app.MapHub<NotificationHub>("/notificationHub");


var scope = app.Services.CreateScope();
var service = scope.ServiceProvider;
try
{
    var context = service.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();
    var userManager = service.GetRequiredService<UserManager<User>>();
    var roleManager = service.GetRequiredService<RoleManager<IdentityRole<int>>>();
    await SeedData.InitializeUser(userManager, roleManager);
}
catch (Exception ex)
{
    var logger = service.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred during migration");
}


app.Run();