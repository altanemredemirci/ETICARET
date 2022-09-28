using ETICARET.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETICARET.DataAccess.Concrete.EfCore
{
    public static class SeedDatabase
    {
        public static void Seed()
        {
            var context = new DataContext();

            if (context.Database.GetPendingMigrations().Count() == 0)
            {
                if (context.Categories.Count() == 0)
                {
                    context.AddRange(Categories);
                }
                if (context.Products.Count() == 0)
                    context.AddRange(Products);

                context.SaveChanges();
            }
        }

        private static Category[] Categories =
        {
            new Category(){Name="Telefon"},
            new Category(){Name="Bilgisayar"}
        };
        private static Product[] Products =
        {
            new Product(){ Name="Samsung Note6", Price=15000, ImageUrl="1.jpg"},
            new Product(){ Name="Samsung Note7", Price=16000, ImageUrl="2.jpg"},
            new Product(){ Name="Samsung Note8", Price=17000, ImageUrl="3.jpg"},
            new Product(){ Name="Samsung Note9", Price=18000, ImageUrl="4.jpg"},
            new Product(){ Name="Samsung Note10", Price=19000, ImageUrl="5.jpg"},
            new Product(){ Name="Samsung Note11", Price=20000, ImageUrl="6.jpg"},
            new Product(){ Name="Samsung Note12", Price=28000, ImageUrl="7.jpg"}
        };
    }
}
