using ETICARET.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DataAccess.Abstract
{
    public interface ICategoryDal : IRepository<Category>
    {
        void DeleteFromCatefory(int categoryId, int productId);
        Category GetByIdWithProducts(int id);
    }
}
