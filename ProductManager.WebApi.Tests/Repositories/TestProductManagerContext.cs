﻿using ProductManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProductManager.Entities;
using System.Data.Entity;

namespace ProductManager.WebApi.Tests
{
    public class TestProductManagerContext : IProductManagerContext
    {
        public TestProductManagerContext()
        {
            this.Employees = new TestDbSetEmployees();
            this.Categories = new TestDbSetCategories();
            this.SubCategories = new TestDbSetSubCategories();
            this.Products = new TestDbSetProducts();
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<Product> Products { get; set; }

        public void Dispose()
        {
        }

        public int SaveChanges()
        {
            return 0;
        }

        public void Initialize()
        {
            var employees = new List<Employee> {
                new Employee { EmployeeId = 1, Title = Entities.Enums.PersonTitle.Mr, FirstName = "George", MiddleName = "Francis", LastName = "Thomas", Email = "georgeft@example.com", },
                new Employee { EmployeeId = 2, Title = Entities.Enums.PersonTitle.Dr, FirstName = "John",MiddleName = "Walter",LastName = "Smith",Email = "johnws@example.com"}
            };
            employees.ForEach(e => this.Employees.Add(e));


            var categories = new List<Category>() {
                new Category { CategoryId = 1, Name = "Clothing" },
                new Category { CategoryId = 2, Name = "Footwear" },
                new Category { CategoryId = 3, Name = "Acessories" }
            };
            categories.ForEach(c => this.Categories.Add(c));


            var subCategories = new List<SubCategory> {
                new SubCategory { SubCategoryId = 1, Name = "Shirts", CategoryId = 1 },
                new SubCategory { SubCategoryId = 2, Name = "Jeans", CategoryId = 1 },
                new SubCategory { SubCategoryId = 3, Name = "Socks", CategoryId = 1 },
                new SubCategory { SubCategoryId = 4, Name = "Shoes", CategoryId = 2 },
                new SubCategory { SubCategoryId = 5, Name = "Trainers", CategoryId = 2 },
                new SubCategory { SubCategoryId = 6, Name = "Bags", CategoryId = 3 },
                new SubCategory { SubCategoryId = 7, Name = "Belts", CategoryId = 3 },
                new SubCategory { SubCategoryId = 8, Name = "Sunglasses", CategoryId = 3 },
                new SubCategory { SubCategoryId = 9, Name = "Watches", CategoryId = 3 }
            };
            subCategories.ForEach(s => this.SubCategories.Add(s));

            var products = new List<Product> {         
                new Product { ProductId = 1,  Name = "Black V Neck Shirt",  ProductNumber = "PCMVNJM11",  ListPrice = (decimal)32.99, StandardCost = (decimal)6.50, SubCategoryId = 1, EmployeeId = 1 }, 
                new Product { ProductId = 2, Name = "Blue Y Neck Shirt", ProductNumber = "PCMYNSM1", ListPrice = (decimal)29.99, StandardCost = (decimal)6.00, SubCategoryId = 1, EmployeeId = 2 },
                new Product { ProductId = 3, Name = "Firetrap Rom Jeans", ProductNumber = "30S38R2", ListPrice = (decimal)65.00, StandardCost = (decimal)14.99, SubCategoryId = 2, EmployeeId = 1 },
                new Product { ProductId = 4, Name = "Airwalk Belted Cargo Jeans", ProductNumber = "0ABCJ36", ListPrice = (decimal)49.99, StandardCost = (decimal)19.99, SubCategoryId = 2, EmployeeId = 1 },
                new Product { ProductId = 5, Name = "Kangol Fashion Jeans", ProductNumber = "31L13MG", ListPrice = (decimal)69.99, StandardCost = (decimal)15.00, SubCategoryId = 2, EmployeeId = 1 }, 
                new Product { ProductId = 6, Name = "Everlast 3 Pack Crew Socks", ProductNumber = "31L13MG", ListPrice = (decimal)10.99, StandardCost = (decimal)2.00, SubCategoryId = 3, EmployeeId = 2 },
                new Product { ProductId = 7, Name = "Dunlop Canvas Low Top Shoes", ProductNumber = "35741842", ListPrice = (decimal)37.99, StandardCost = (decimal)10.49, SubCategoryId = 4, EmployeeId = 1 }, 
                new Product { ProductId = 8, Name = "Giorgio Bourne Slip On Shoes", ProductNumber = "26391347", ListPrice = (decimal)29.99, StandardCost = (decimal)13.00, SubCategoryId = 4, EmployeeId = 1},
                new Product { ProductId = 9, Name = "Firetrap Spencer Shoes", ProductNumber = "07411145", ListPrice = (decimal)100.00, StandardCost = (decimal)35.00, SubCategoryId = 4, EmployeeId = 1 },
                new Product { ProductId = 10, Name = "Kickers Fragma Lace Shoes", ProductNumber = "0KFL", ListPrice = (decimal)54.99, StandardCost = (decimal)44.99, SubCategoryId = 4, EmployeeId = 2 }, 
                new Product { ProductId = 11, Name = "Kangol Waltham Slip Shoes", ProductNumber = "2741126465", ListPrice = (decimal)44.99, StandardCost = (decimal)20.00, SubCategoryId = 4, EmployeeId = 1 }, 
                new Product { ProductId = 12, Name = "Converse All Stars Ox Trainers", ProductNumber = "3233512", ListPrice = (decimal)44.99, StandardCost = (decimal)33.00, SubCategoryId = 5, EmployeeId = 2 }, 
                new Product { ProductId = 13, Name = "Adidas Galaxy Elite Traineirs", ProductNumber = "740712475", ListPrice = (decimal)39.99, StandardCost = (decimal)28.00, SubCategoryId = 5, EmployeeId = 1 }, 
                new Product { ProductId = 14, Name = "Nike Air Max Ivo Trainers", ProductNumber = "7666776886", ListPrice = (decimal)94.99, StandardCost = (decimal)74.99, SubCategoryId = 5, EmployeeId = 2 }, 
                new Product { ProductId = 15, Name = "Nike Tanjun Trainers", ProductNumber = "6401146", ListPrice = (decimal)54.99, StandardCost = (decimal)44.99, SubCategoryId = 5, EmployeeId = 1 }, 
                new Product { ProductId = 16, Name = "Salomon XR Shift Trail Traineirs", ProductNumber = "842SXRS", ListPrice = (decimal)79.99, StandardCost = (decimal)40.00, SubCategoryId = 5, EmployeeId = 2 }, 
                new Product { ProductId = 17, Name = "Kangol Embossed Belt", ProductNumber = "7KEBSM2XL", ListPrice = (decimal)21.99, StandardCost = (decimal)5.75, SubCategoryId = 7, EmployeeId = 2 },
                new Product { ProductId = 18, Name = "Lee Cooper Stitched Belt", ProductNumber = "FR0LC4", ListPrice = (decimal)9.99, StandardCost = (decimal)3.50, SubCategoryId = 7, EmployeeId = 1 },
                new Product { ProductId = 19, Name = "Dunlop Golf Sunglasses", ProductNumber = "5OE09ZI", ListPrice = (decimal)14.99, StandardCost = (decimal)5.99, SubCategoryId = 8, EmployeeId = 1 }, 
                new Product { ProductId = 20, Name = "Garmin Forerunner 220 Watch", ProductNumber = "0220GFW", ListPrice = (decimal)159.99, StandardCost = (decimal)127.99, SubCategoryId = 9, EmployeeId = 1 }, 
                new Product { ProductId = 21, Name = "Suunto Core Watch", ProductNumber = "BBBGGM", ListPrice = (decimal)224.99, StandardCost = (decimal)179.99, SubCategoryId = 9, EmployeeId = 1 }
            };
            products.ForEach(p => this.Products.Add(p));
        }
    }
}
