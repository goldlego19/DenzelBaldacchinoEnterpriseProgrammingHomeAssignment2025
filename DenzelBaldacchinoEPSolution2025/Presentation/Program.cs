using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DataAccess.DataContext;
using DataAccess.Repositories;
using Domain.Models;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<PollDbContext>(options =>
    options.UseSqlServer(connectionString)
           .UseLazyLoadingProxies());
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<PollDbContext>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<PollRepository>();




/*
 * AddScoped - 1 instance per user per request
 * AddTransient - 1 instance per user per request per call
 * AddSingleton - 1 instance of StudentsRepository to be shared by all users accessing your website and all requests
 */



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);
app.MapRazorPages();


app.Run();
