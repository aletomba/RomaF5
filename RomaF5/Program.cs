using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RomaF5.Data;
using RomaF5.IRepository;
using RomaF5.Service;
using Rotativa.AspNetCore;


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
builder.Services.AddScoped<ProductoRepo>();
builder.Services.AddScoped<ProveedorRepository>();
builder.Services.AddTransient<IPaginationService,PaginationService>();



var app = builder.Build();


var env = app.Services.GetRequiredService<IWebHostEnvironment>();
if (env.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
RotativaConfiguration.Setup(env.WebRootPath, "rotativa");

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Enable authentication and authorization
app.UseAuthentication();
app.UseAuthorization();
app.UseRotativa();  
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

