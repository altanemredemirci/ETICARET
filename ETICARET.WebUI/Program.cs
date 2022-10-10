using ETICARET.Business.Abstract;
using ETICARET.Business.Concrete;
using ETICARET.DataAccess.Abstract;
using ETICARET.DataAccess.Concrete.EfCore;
using ETICARET.DataAccess.Concrete.Memory;
using ETICARET.WebUI.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

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

app.Run();
