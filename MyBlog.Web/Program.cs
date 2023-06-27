using Microsoft.AspNetCore.Identity;
using MyBlog.Data.Context;
using MyBlog.Data.Extensions;
using MyBlog.Entity.Entities;
using MyBlog.Service.Extensions;
using NToastNotify;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.LoadDataLayerExtension(builder.Configuration);
builder.Services.LoadServiceLayerExtensions(); //MyServiceLayerExtension
builder.Services.AddSession(); //Oturumlar icin
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();

builder.Services.AddControllersWithViews().AddNToastNotifyToastr(new ToastrOptions() //Toastr Service
{
    ProgressBar = true,
    PositionClass = ToastPositions.TopRight,
    CloseButton = true,
    TimeOut = 5000

}).AddRazorRuntimeCompilation();

//-------------------------------DÝKKAT-------------------------------
//CANLI ÖNCESÝ BURAYI KALDIR
builder.Services.AddIdentity<AppUser, AppRole>(
    opt =>
    {
        opt.Password.RequireNonAlphanumeric = false;
        opt.Password.RequireLowercase = false;
        opt.Password.RequireUppercase = false;
    })
    .AddRoleManager<RoleManager<AppRole>>().
    AddEntityFrameworkStores<AppDbContext>().
    AddDefaultTokenProviders();
//-------------------------------DÝKKAT-------------------------------
//-------------------------------DÝKKAT-------------------------------
builder.Services.ConfigureApplicationCookie(config =>
{
    config.LoginPath = new PathString("/Admin/Auth/Login"); //Area/[controller]/[action] => Biri giriþ yapmamýþsa bu sayfaya yönlendirsin diye.
    config.LogoutPath = new PathString("/Admin/Auth/Logout");
    config.Cookie = new CookieBuilder
    {
        Name = "tayfunfirtina",
        HttpOnly = true,
        SameSite = SameSiteMode.Strict,
        SecurePolicy = CookieSecurePolicy.SameAsRequest //CANLIDA BURAYI "always" olarak deðiþtirmeyi unutma
    };
    config.SlidingExpiration = true;
    config.ExpireTimeSpan = TimeSpan.FromDays(7);
    config.AccessDeniedPath = new PathString("/Admin/Auth/AccessDenied"); // => Yetkisiz yapýlmak istenen iþlemlerde bu sayfaya yönlendiricem.
});
//-------------------------------DÝKKAT-------------------------------


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseNToastNotify(); //Toastr icin

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession(); //Oturumlar icin

app.UseRouting();
app.UseAuthentication();//Her zaman UseAuthorizationun ustunde olmasi lazim
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

//My Areas EndPoint
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
      name: "areas",
      pattern: "{area:exists}/{controller=Default}/{action=Index}/{id?}"
    );
});

//Tek bir Area olacaksa asagidaki kodu kullanabilirim.
//Bu kodu kullanirsam Area-Admin tarafindaki Controllerda Route[....] helperini kullanmama gerek kalmaz.
//app.UseEndpoints(endpoints =>
//{
//    endpoints.MapAreaControllerRoute(
//    name: "Admin",
//    areaName: "Admin",
//    pattern: "Admin/{controller=Home}/{action=Index}/{id?}"
//    );
//    endpoints.MapDefaultControllerRoute();
//});

app.Run();
