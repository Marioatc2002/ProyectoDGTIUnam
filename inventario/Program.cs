using inventario.Data;
using inventario.Models;
using inventario.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();



builder.Services.AddDbContext<AppDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddDistributedMemoryCache(); // Cache en memoria
builder.Services.Configure<TwilioSettings>(builder.Configuration.GetSection("Twilio"));
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));
// Registramos el modelo de configuración
builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

// Registramos el servicio de Gmail
builder.Services.AddTransient<IEmailService, GmailEmailService>();
builder.Services.AddTransient<ISmsService, TwilioSmsService>();
// Esto mapea la sección "Twilio" de appsettings.json directamente a la clase
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
{ 
    options.LoginPath = "/Login/IniciarSesion";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
});
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add(new ResponseCacheAttribute
    {
        NoStore = true,
        Location = ResponseCacheLocation.None,
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();


app.UseAuthentication();

app.UseAuthorization();
 
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
