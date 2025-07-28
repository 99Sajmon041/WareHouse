using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WareHouseSTARNET;
using WareHouseSTARNET.Data;
using WareHouseSTARNET.Models;
using WareHouseSTARNET.Repositories.Implementations;
using WareHouseSTARNET.Repositories.Interfaces;
using WareHouseSTARNET.Services.Implementations;
using WareHouseSTARNET.Services.Interfaces;
using WareHouseSTARNET.UserServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(typeof(MappingProfile).Assembly);

builder.Services.AddScoped<IMaterialRepository, MaterialRepository>();
builder.Services.AddScoped<ITypeOfMaterialRepository, TypeOfMaterialRepository>();
builder.Services.AddScoped<IWrittenOffMaterialRepository, WrittenOffMaterialRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

builder.Services.AddScoped<IMaterialService, MaterialService>();
builder.Services.AddScoped<ITypeOfMaterialService, TypeOfMaterialService>();
builder.Services.AddScoped<IWrittenOffMaterialService, WrittenOffMaterialService>();
builder.Services.AddScoped<IApplicationUserService, ApplicationUserService>();
builder.Services.AddScoped<IFormHelperService, FormHelperService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<SeedService>();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});

builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(3333);
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 6;
});


var app = builder.Build();

var cultureInfo = new CultureInfo("cs-CZ");
cultureInfo.NumberFormat.NumberDecimalSeparator = ",";
CultureInfo.DefaultThreadCurrentCulture = cultureInfo;
CultureInfo.DefaultThreadCurrentUICulture = cultureInfo;

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedService = services.GetRequiredService<SeedService>();
    await seedService.InitializeAsync();
}

app.Run();