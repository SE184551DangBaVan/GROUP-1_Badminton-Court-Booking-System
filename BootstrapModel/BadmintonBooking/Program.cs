using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BadmintonBooking.Data;
using Microsoft.Extensions.Options;
using demobadminton.Repository.Service;
using demobadminton.Repository.Interface;
using BadmintonBooking.Models;
using BadmintonBooking.SignalR;
var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("BadmintonBookingIdentityContextConnection") ?? throw new InvalidOperationException("Connection string 'BadmintonBookingIdentityContextConnection' not found.");

builder.Services.AddDbContext<BadmintonBookingIdentityContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<DemobadmintonContext>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => {options.SignIn.RequireConfirmedAccount = true; options.User.AllowedUserNameCharacters= "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ "; options.SignIn.RequireConfirmedEmail = true;
}).AddRoles<IdentityRole>().AddEntityFrameworkStores<BadmintonBookingIdentityContext>().AddDefaultTokenProviders();
builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.ConfigureApplicationCookie(options =>
{

    options.AccessDeniedPath = "/Account/AccessDenied";
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.Cookie.Name = "BadmintonBooking";
    options.ExpireTimeSpan = TimeSpan.FromDays(30);
    options.SlidingExpiration = true;

});
builder.Services.Configure<SecurityStampValidatorOptions>(o =>
    o.ValidationInterval = TimeSpan.FromMinutes(0));
//For google authentication
builder.Services.AddAuthentication()
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Auth:Google:ClientId"];
        options.ClientSecret = builder.Configuration["Auth:Google:ClientSecret"];
        options.Events.OnRemoteFailure = context =>
        {
            context.Response.Redirect("/Account/Login");
            context.HandleResponse();
            return Task.CompletedTask;
        };

    });


//For paypal
builder.Services.AddHttpContextAccessor();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddSignalR();

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
//session for paypal id
app.UseSession();


app.UseAuthentication();
app.UseAuthorization();
// clear cache 
// Middleware to set cache control headers
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "-1";

    await next();
});

//
app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapHub<SessionHub>("/sessionHub");

app.Run();
