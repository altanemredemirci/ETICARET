using ETICARET.Business.Abstract;
using ETICARET.Business.Concrete;
using ETICARET.DataAccess.Abstract;
using ETICARET.DataAccess.Concrete.EfCore;
using ETICARET.DataAccess.Concrete.Memory;
using ETICARET.WebUI.Identity;
using ETICARET.WebUI.Middlewares;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<ApplicationIdentityDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("IdentityConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationIdentityDbContext>()
    .AddDefaultTokenProviders();

//SeedIdentity parametres 
var userManager = builder.Services.BuildServiceProvider().GetService<UserManager<ApplicationUser>>();
var roleManager = builder.Services.BuildServiceProvider().GetService<RoleManager<IdentityRole>>();


builder.Services.Configure<IdentityOptions>(options =>
{
    //password

    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireDigit = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 6;

    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.AllowedForNewUsers = true;

    options.User.RequireUniqueEmail = true;
    options.SignIn.RequireConfirmedEmail = true;
    options.SignIn.RequireConfirmedPhoneNumber = false;


});

builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/account/accessdenied";
    options.LoginPath = "/Account/login";
    options.LogoutPath = "/Account/logout";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.SlidingExpiration = true;
    options.Cookie = new CookieBuilder()
    {
        HttpOnly = true,
        Name = "ETICARET.Security.Cookie",
        SameSite = SameSiteMode.Strict
    };
});




builder.Services.AddScoped<IProductDal, EfCoreProductDal>();
builder.Services.AddScoped<IProductService, ProductManager>();
builder.Services.AddScoped<ICategoryDal, EfCoreCategoryDal>();
builder.Services.AddScoped<ICategoryService, CategoryManager>();



//builder.Services.AddControllersWithViews(); MVC import

//MVC Mimarisi Tanýmlandý.
builder.Services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Latest);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

SeedDatabase.Seed();
app.UseStaticFiles();
app.CustomStaticFiles(); //middleware:Bootstrap kütüphanesini npm aracýlýðýyla static dosya olarak projeye daihl edeceðiz.
app.UseAuthentication();
app.UseRouting();

app.UseAuthorization();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}");

    endpoints.MapControllerRoute
    (
        name: "products",
        pattern: "products/{category?}",
        defaults: new { controller = "Shop", action = "List" }
    );

    endpoints.MapControllerRoute
(
    name: "adminProducts",
    pattern: "admin/products",
    defaults: new { controller = "Admin", action = "ProductList" }
);

    endpoints.MapControllerRoute
(
name: "adminProducts",
pattern: "admin/products/{id?}",
defaults: new { controller = "Admin", action = "EditProduct" }
);

    endpoints.MapControllerRoute
(
  name: "adminCategories",
  pattern: "admin/categories",
  defaults: new { controller = "Admin", action = "CategoryList" }
);

    endpoints.MapControllerRoute
(
  name: "adminCategories",
  pattern: "admin/categories/{id?}",
  defaults: new { controller = "Admin", action = "EditCategory" }
);


});


SeedIdentity.Seed(userManager, roleManager, app.Configuration).Wait();

app.Run();
