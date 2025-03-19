using JarredsOrderHub.Controllers.Service;
using JarredsOrderHub.DbaseContext;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Configurar el servicio de base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Agregar soporte para controladores con vistas
builder.Services.AddControllersWithViews();

// Agregar autenticación con cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuario/AccionesUsuario";  // Ruta donde se encuentra el login
        options.AccessDeniedPath = "/Home";  // Si el usuario no tiene permisos
    });

// Agregar autorización
builder.Services.AddAuthorization();

//Soporte para acceder al HttpContext en servicios
builder.Services.AddHttpContextAccessor();

// Servicios
builder.Services.AddScoped<RolService>();
builder.Services.AddScoped<UsuarioService>();
builder.Services.AddScoped<CatalogoService>();
builder.Services.AddScoped<AuditService>();
builder.Services.AddScoped<EmailService>();

//Agregar sesiones 
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always; // Asegura que la cookie solo se envíe vía HTTPS
});

builder.Services.AddAntiforgery(options =>
{
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
});

var app = builder.Build();

// Configurar el pipeline de middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseSession();  // Sesiones habilitadas
app.UseAuthentication();
app.UseAuthorization();

// Configurar rutas
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Usuario}/{action=AccionesUsuario}/{id?}");

app.Run();
