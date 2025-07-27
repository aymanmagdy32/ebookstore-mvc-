using BookShoppingCartMvcUI;
using BookShoppingCartMvcUI.Constants;
using BookShoppingCartMvcUI.Data;
using BookShoppingCartMvcUI.Repo;
using BookShoppingCartMvcUI.Repositories;
using BookShoppingCartMvcUI.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
    options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IHomeRepo, HomeRepo>();
builder.Services.AddTransient<ICartRepo, CartRepo>();
builder.Services.AddTransient<IUserOrderRepo, UserOrderRepo>();
builder.Services.AddTransient<IStockRepository, StockRepo>();
builder.Services.AddTransient<IFileService, FileService>();
builder.Services.AddTransient<IBookRepo, BookRepo>();
builder.Services.AddTransient<IGenreRepo, GenreRepo>();


var app = builder.Build();

//using(var scope = app.Services.CreateScope()) 
//{

// await DbSeeder.SeedDefaultData(scope.ServiceProvider);
//}


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
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
