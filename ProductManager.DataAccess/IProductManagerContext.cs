using System.Data.Entity;
using ProductManager.Entities;
using System;
using System.Threading.Tasks;

namespace ProductManager.DataAccess
{
    public interface IProductManagerContext : IDisposable
    {
        DbSet<Employee> Employees { get; set; }
        DbSet<Category> Categories { get; set; }
        DbSet<SubCategory> SubCategories { get; set; }
        DbSet<Product> Products { get; set; }
        int SaveChanges();
    }
}