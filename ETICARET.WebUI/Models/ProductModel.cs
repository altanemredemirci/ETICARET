using ETICARET.Entities;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace ETICARET.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(60,MinimumLength =5,ErrorMessage ="Ürün ismi minimum 10 maximum 60 karakter olmalıdır.")]
        public string Name { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [StringLength(100, MinimumLength = 10, ErrorMessage = "Ürün açıklaması minimum 10 maximum 60 karakter olmalıdır.")]
        public string Description { get; set; }
        public List<Image> Images { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Range(10000,40000)]
        public decimal Price { get; set; }

        //public List<ProductCategory> ProductCategories { get; set; }
        public List<Category> SelectedCategories { get; set; }

        public ProductModel()
        {
            Images = new List<Image>();
        }
    }
}
