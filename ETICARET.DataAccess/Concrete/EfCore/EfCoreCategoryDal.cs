using ETICARET.DataAccess.Abstract;
using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public class EfCoreCategoryDal : EfCoreGenericRepository<Category, DataContext>, ICategoryDal
    {
        public override void Delete(Category entity)
        {
            using (var context= new DataContext())
            {
                context.Categories.Remove(entity);
                context.ProductCategories.RemoveRange(entity.ProductCategories);
                context.SaveChanges();
            }
                
        }

        public Category GetByIdWithProducts(int id)
        {
            using(var context = new DataContext())
            {
                return context.Categories
                        .Where(i => i.Id == id)
                        .Include(i => i.ProductCategories)
                        .ThenInclude(i => i.Product)
                        .FirstOrDefault();
            }
        }
    }
}
