using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.Entities
{
    public class ProductCategory
    {
        //public int Id { get; set; }

        public int CategoryId { get; set; }
        public Category Category { get; set; } //Mapping

        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
