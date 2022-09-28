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
        
        public IActionResult List()
        {
            return View(new ProductListModel()
            {
                Products=_productService.GetAll()
            });
        }

        public IActionResult Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            //Product product = _productService.GetById((int)id); 
            Product product = _productService.GetById(id.Value); 

            if(product==null)
            {
                return NotFound();
            }


            return View(product);
        }
    }
}
