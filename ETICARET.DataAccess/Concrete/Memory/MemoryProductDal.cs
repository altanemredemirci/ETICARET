using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DataAccess.Concrete.Memory
{
    public class MemoryProductDal //: IProductDal
    {
        public void Create(Product entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(Product entity)
        {
            throw new NotImplementedException();
        }

        //public IEnumerable<Product> GetAll(Expression<Func<Product, bool>> filter = null)
        //{
        //    var products = new List<Product>()
        //   {
        //       //new Product(){ Id=1,Name="Samsung Note9", Price=15000, ImageUrl="1.jpg"},
        //       //new Product(){ Id=2,Name="Samsung Note10", Price=16000, ImageUrl="2.jpg"},
        //       //new Product(){ Id=3,Name="Samsung Note11", Price=17000, ImageUrl="3.jpg"}
        //   };

        //    return products;
        //}

        public Product GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Product GetOne(Expression<Func<Product, bool>> filter)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Product> GetPopularProducts()
        {
            throw new NotImplementedException();
        }

        public void Update(Product entity)
        {
            throw new NotImplementedException();
        }
    }
}
