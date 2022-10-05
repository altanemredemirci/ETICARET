using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IProductService _productService;

        public AdminController(IProductService productService)
        {
            _productService = productService;
        }

        [Route("admin/products")]
        public IActionResult ProductList()
        {
            return View( new ProductListModel()
            {
                Products=_productService.GetAll()
            });
        }

        public IActionResult CreateProduct()
        {
            return View(new ProductModel());
        }

        [HttpPost]
        public IActionResult CreateProduct(ProductModel model)
        {
            var entity = new Product()
            {
                Name=model.Name,
                Description=model.Description,
                Price=model.Price,
                Images= new List<Image>() { new Image() { ImageUrl="2.jpg"} }
            };

            _productService.Create(entity);
            return Redirect("Index");
        }

        public IActionResult EditProduct(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var entity = _productService.GetProductDetails(id);

            if(entity==null)
                return NotFound();

            var model = new ProductModel()
            {
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Images = entity.Images
            };
            return View(model);
        }

        [HttpPost]
        public IActionResult EditProduct(ProductModel model)
        {
            return View();
        }
    }
}
