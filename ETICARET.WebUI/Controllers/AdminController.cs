using ETICARET.Business.Abstract;
using ETICARET.Entities;
using ETICARET.WebUI.Models;
using Microsoft.AspNetCore.Mvc;

namespace ETICARET.WebUI.Controllers
{
    public class AdminController : Controller
    {
        IProductService _productService;
        ICategoryService _categoryService;

        public AdminController(IProductService productService,ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
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
        public async Task<IActionResult> CreateProduct(ProductModel model,List<IFormFile> files)
        {
            if (ModelState.IsValid)
            {
                var entity = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price
                };

                if (files != null)
                {
                    foreach (var file in files)
                    {
                        Image image = new Image();
                        image.ImageUrl = file.FileName;

                        entity.Images.Add(image);

                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                        using (var stream = new FileStream(path, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }

                    }
                }

                _productService.Create(entity);
                return Redirect("/admin/products");
            }
            return View(model);
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
                Id=entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Price = entity.Price,
                Images = entity.Images,
                SelectedCategories=entity.ProductCategories.Select(i=> i.Category).ToList()
            };

            ViewBag.Categories = _categoryService.GetAll();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditProduct(ProductModel model, List<IFormFile> files, int[] categoryIds)
        {

            var entity = _productService.GetById(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;
            entity.Description = model.Description;
            entity.Price = model.Price; 

            if (files != null)
            {
                foreach (var file in files)
                {
                    Image image = new Image();
                    image.ImageUrl = file.FileName;

                    entity.Images.Add(image);

                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", file.FileName);

                    using (var stream = new FileStream(path, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                }

                _productService.Update(entity,categoryIds);
                return RedirectToAction("ProductList");
            }

            return View(entity);
        }

        public IActionResult DeleteProduct(int productId)
        {
            var product = _productService.GetById(productId);

            _productService.Delete(product);

            return RedirectToAction("ProductList");
        }

        [Route("admin/categories")]
        public IActionResult CategoryList()
        {
            return View(new CategoryListModel()
            {
                Categories = _categoryService.GetAll()
            });
        }

        public IActionResult CreateCategory()
        {
            return View(new CategoryModel());
        }
        [HttpPost]
        public IActionResult CreateCategory(CategoryModel model)
        {
            var entity = new Category()
            {
                Name = model.Name
            };

            _categoryService.Create(entity);
            return RedirectToAction("CategoryList");
        }

        public IActionResult EditCategory(int? id)
        {
            var entity = _categoryService.GetByIdWithProducts(id.Value); //(int)id

            return View(new CategoryModel()
            {
                Id=entity.Id,
                Name=entity.Name,
                Products=entity.ProductCategories.Select(i=> i.Product).ToList()
            });
        }
        [HttpPost]
        public IActionResult EditCategory(CategoryModel model)
        {
            var entity = _categoryService.GetById(model.Id);

            if (entity == null)
            {
                return NotFound();
            }

            entity.Name = model.Name;

            _categoryService.Update(entity);
            return RedirectToAction("CategoryList");
        }

        [HttpPost]
        public IActionResult DeleteCategory(int categoryId)
        {
            var entity = _categoryService.GetById(categoryId);

            _categoryService.Delete(entity);
            return RedirectToAction("CategoryList");
        }


        [HttpPost]
        public IActionResult DeleteFromCategory(int categoryId, int productId)
        {
            _categoryService.DeleteFromCatefory(categoryId,productId);

           
            return Redirect("/admin/categories/" + categoryId);
        }
    }
}
