using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BadmintonBooking.Data;
using Microsoft.Extensions.Options;
using demobadminton.Repository.Service;
using demobadminton.Repository.Interface;
using BadmintonBooking.Models;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BadmintonBookingIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'BadmintonBookingIdentityContextConnection' not found.");

builder.Services.AddDbContext<BadmintonBookingIdentityContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<DemobadmintonContext>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => {options.SignIn.RequireConfirmedAccount = true;options.SignIn.RequireConfirmedEmail = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<BadmintonBookingIdentityContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.ConfigureApplicationCookie(options =>
{

    options.AccessDeniedPath = "/Account/Login";
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.Cookie.Name = "BadmintonBooking";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

});

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
