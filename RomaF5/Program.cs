using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.IRepository;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


// Configure the DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlite(builder.Configuration.GetConnectionString("SQLiteConnection"));
});
// Configure Identity services

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
}).AddRoles<IdentityRole>()
  .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Configura la ruta de inicio de sesión
        options.LogoutPath = "/Account/Logout"; // Configura la ruta de cierre de sesión
        options.AccessDeniedPath = "/Home/Privacy"; // Configura la ruta de acceso denegado
        options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    });

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<TurnoRepository>();
builder.Services.AddScoped<VentaRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
else
{
    // En producción, podemos personalizar cómo manejar las excepciones sin redirigir a la página de errores estándar.
    app.UseExceptionHandler(errorApp =>
    {
        errorApp.Run(async context =>
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/html";

            await context.Response.WriteAsync("<html><body>\r\n");
            await context.Response.WriteAsync("Ha ocurrido un error en el servidor. Por favor, intenta nuevamente más tarde.\r\n");
            await context.Response.WriteAsync("</body></html>\r\n");
        });
    });
    app.UseHsts();
}

// Resto de la configuración del pipeline


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
   
// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");
 

app.Run();

