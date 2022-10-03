using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class ShopController : Controller
    {
        private IProductService _productService;

        public ShopController(IProductService productService)
        {
            _productService = productService;
        }

        //products/telefon?page=2
        [Route("products/{category?}")]
        public IActionResult List(string category, int page=1)
        {
            const int pageSize = 3;

            return View(new ProductListModel()
            {
                PageInfo = new PageInfo()
                {
                    TotalItems=_productService.GetCountByCategory(category),
                    ItemsPerPage=pageSize,
                    CurrentCategory=category,
                    CurrentPage=page
                },

                Products=_productService.GetProductsByCategory(category, page,pageSize)
            });
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Product product = _productService.GetById((int)id); 
            Product product = _productService.GetProductDetails(id.Value);

            if (product == null)
                return NotFound();

            return View(new ProductDetailsModel()
            {
                Product = product,
                Categories = product.ProductCategories.Select(i => i.Category).ToList()
            });
          
        }
    }
}
