using ProductManager.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductManager.DataAccess
{
    public class ProductManagerContext : DbContext, IProductManagerContext
    {
        public ProductManagerContext() : base("ProductManager")
        {

        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}
