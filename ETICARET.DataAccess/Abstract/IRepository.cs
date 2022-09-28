using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DataAccess.Abstract
{
    public interface IRepository<T> 
    {
        T GetById(int id);
        T GetOne(Expression<Func<T, bool>> filter);
        List<T> GetAll(Expression<Func<T, bool>> filter=null);

        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);


        #region ICollection
        //var products =db.Products.AsQueryable();
        //if(user!=null)
        //        products.Where(i=> i.Owner==user.Username).ToList()


        //List() : Sorgu çekildikçe DB gider ve Selext * from Komutunu DB çalıştırır.
        //IQueryable : İlk sorgu sonrası getirilen datayı ram bellekte tutar. ** Sorgu yazdıktan sonra tekrar koşul eklenebilir.
        //IEnumerable : İlk sorgu sonrası getirilen datayı ram bellekte tutar.
        #endregion


    }
}
