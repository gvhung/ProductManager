using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Simple.OData.Client;
using ProductManager.WebApi.Controllers;
using System.Collections.Generic;
using ProductManager.Entities;
using ProductManager.DataAccess;
using System.Web.Http.Results;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web.OData;
using System.Web.OData.Results;
using System.Web.Http;
using System.Net;


namespace ProductManager.WebApi.Tests
{
    [TestClass]
    public class TestProductsController
    {
        private readonly TestProductManagerContext context = new TestProductManagerContext();

        public TestProductsController()
        {
            context.Initialize();
        }

        [TestMethod]
        public void GetAllProducts()
        {
            var controller = new ProductsController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetProducts();

            Assert.AreEqual(context.Products.Local.Count, result.Count());
        }

        [TestMethod]
        public void GetSingleProduct()
        {
            int id = new Random().Next(1, context.Products.Local.Count + 1);

            var controller = new ProductsController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetProduct(id).Queryable.Single();

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.ProductId);
        }

        [TestMethod]
        public void PutProduct()
        {
            int id = new Random().Next(1, context.Products.Local.Count + 1);

            //Recolhe dados antes da alteração
            var oldProduct = context.Products.Find(id);
            //Guardar em variáveis para os testes
            var oldProductId = oldProduct.ProductId;
            var oldName = oldProduct.Name;
            var oldProductNumber = oldProduct.ProductNumber;
            var oldListPrice = oldProduct.ListPrice;
            var oldStandardCost = oldProduct.StandardCost;
            var oldSubCategoryId = oldProduct.SubCategoryId;
            var oldEmployeeId = oldProduct.EmployeeId;
            var oldModifiedDate = oldProduct.ModifiedDate;

            //Calcular nova SubCategoria aleatoria diferente da anterior         
            int newSubCategoryId = new Random().Next(1, context.SubCategories.Local.Count + 1);
            while (newSubCategoryId == oldSubCategoryId)
            {
                newSubCategoryId = new Random().Next(1, context.SubCategories.Local.Count + 1);
            }

            //Calcular novo Empregado aleatorio diferente do anterior         
            int newEmployeeId = new Random().Next(1, context.Employees.Local.Count + 1);
            while (newEmployeeId == oldEmployeeId)
            {
                newEmployeeId = new Random().Next(1, context.Employees.Local.Count + 1);
            }

            var delta = new Delta<Product>(typeof(Product));
            delta.TrySetPropertyValue("ProductId", id);
            delta.TrySetPropertyValue("Name", "Modified:" + oldName);
            delta.TrySetPropertyValue("ProductNumber", "Modified:" + oldProductNumber);
            delta.TrySetPropertyValue("ListPrice", 1 + oldListPrice);
            delta.TrySetPropertyValue("StandardCost", 1 + oldStandardCost);
            delta.TrySetPropertyValue("SubCategoryId", newSubCategoryId);
            delta.TrySetPropertyValue("EmployeeId", newEmployeeId);


            var controller = new ProductsController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Put(id, delta) as UpdatedODataResult<Product>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldProductId, result.Entity.ProductId);
            Assert.AreNotEqual(oldName, result.Entity.Name);
            Assert.AreNotEqual(oldProductNumber, result.Entity.ProductNumber);
            Assert.AreNotEqual(oldListPrice, result.Entity.ListPrice);
            Assert.AreNotEqual(oldStandardCost, result.Entity.StandardCost);
            Assert.AreNotEqual(oldSubCategoryId, result.Entity.SubCategoryId);
            Assert.AreNotEqual(oldEmployeeId, result.Entity.EmployeeId);
            Assert.AreNotEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void PostProduct()
        {
            var id = context.Products.Local.Count + 1;

            Product newProduct = new Product {
                ProductId = id,
                Name = "New Product Name",
                ProductNumber = "0NEW123PNO",
                ListPrice = (decimal)19.99,
                StandardCost = (decimal)5.00,
                SubCategoryId = new Random().Next(1, context.SubCategories.Local.Count + 1),
                EmployeeId = new Random().Next(1, context.Employees.Local.Count + 1)
            };
        

            var controller = new ProductsController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Post(newProduct) as CreatedODataResult<Product>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(id, result.Entity.ProductId);
            Assert.AreNotEqual(context.Products.Local.Count + 1, result.Entity.ProductId);
        }

        [TestMethod]
        public void PatchProduct()
        {
            int id = new Random().Next(1, context.Products.Local.Count + 1);

            //Recolhe dados antes da alteração
            var oldProduct = context.Products.Find(id);
            //Guardar em variáveis para os testes
            var oldProductId = oldProduct.ProductId;
            var oldName = oldProduct.Name;
            var oldProductNumber = oldProduct.ProductNumber;
            var oldListPrice = oldProduct.ListPrice;
            var oldStandardCost = oldProduct.StandardCost;
            var oldSubCategoryId = oldProduct.SubCategoryId;
            var oldEmployeeId = oldProduct.EmployeeId;
            var oldModifiedDate = oldProduct.ModifiedDate;

            //Calcular nova SubCategoria aleatoria diferente da anterior         
            int newSubCategoryId = new Random().Next(1, context.SubCategories.Local.Count + 1);
            while (newSubCategoryId == oldSubCategoryId)
            {
                newSubCategoryId = new Random().Next(1, context.SubCategories.Local.Count + 1);
            }

            //Calcular novo Empregado aleatorio diferente do anterior         
            int newEmployeeId = new Random().Next(1, context.Employees.Local.Count + 1);
            while (newEmployeeId == oldEmployeeId)
            {
                newEmployeeId = new Random().Next(1, context.Employees.Local.Count + 1);
            }

            var delta = new Delta<Product>(typeof(Product));
            delta.TrySetPropertyValue("ProductId", id);
            delta.TrySetPropertyValue("Name", "Modified:" + oldName);
            delta.TrySetPropertyValue("ProductNumber", "Modified:" + oldProductNumber);
            delta.TrySetPropertyValue("ListPrice", 1 + oldListPrice);
            delta.TrySetPropertyValue("StandardCost", 1 + oldStandardCost);
            delta.TrySetPropertyValue("SubCategoryId", newSubCategoryId);
            delta.TrySetPropertyValue("EmployeeId", newEmployeeId);


            var controller = new ProductsController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Patch(id, delta) as UpdatedODataResult<Product>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldProductId, result.Entity.ProductId);
            Assert.AreNotEqual(oldName, result.Entity.Name);
            Assert.AreNotEqual(oldProductNumber, result.Entity.ProductNumber);
            Assert.AreNotEqual(oldListPrice, result.Entity.ListPrice);
            Assert.AreNotEqual(oldStandardCost, result.Entity.StandardCost);
            Assert.AreNotEqual(oldSubCategoryId, result.Entity.SubCategoryId);
            Assert.AreNotEqual(oldEmployeeId, result.Entity.EmployeeId);
            Assert.AreEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void DeleteProduct()
        {
            int id = new Random().Next(1, context.Products.Local.Count + 1);

            var controller = new ProductsController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Delete(id) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.IsNull(context.Products.Find(id));
        }
    }
}
