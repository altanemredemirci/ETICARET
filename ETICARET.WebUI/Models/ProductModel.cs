using ETICARET.Entities;

namespace ETICARET.WebUI.Models
{
    public class ProductModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Image> Images { get; set; }
        public decimal Price { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }

        public ProductModel()
        {
            Images = new List<Image>();
        }
    }
}
