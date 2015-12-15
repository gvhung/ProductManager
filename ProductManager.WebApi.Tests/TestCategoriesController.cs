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
    public class TestCategoriesController
    {
        private readonly TestProductManagerContext context = new TestProductManagerContext();

        public TestCategoriesController()
        {
            context.Initialize();
        }

        [TestMethod]
        public void GetAllCategories()
        {
            var controller = new CategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetCategories();

            Assert.AreEqual(context.Categories.Local.Count, result.Count());
        }

        [TestMethod]
        public void GetSingleCategory()
        {
            int id = new Random().Next(1, context.Categories.Local.Count + 1);

            var controller = new CategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetCategory(id).Queryable.Single();

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.CategoryId);
        }


        [TestMethod]
        public void PutCategory()
        {
            int id = new Random().Next(1, context.Categories.Local.Count + 1);
            
            //Recolhe dados antes da alteração
            var oldCategory = context.Categories.Find(id);
            //Guardar em variáveis para os testes
            var oldCategoryId = oldCategory.CategoryId;
            var oldName = oldCategory.Name;
            var oldModifiedDate = oldCategory.ModifiedDate;


            var delta = new Delta<Category>(typeof(Category));
            delta.TrySetPropertyValue("CategoryId", id);
            delta.TrySetPropertyValue("Name", "Modified:" + oldName);

            var controller = new CategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Put(id, delta) as UpdatedODataResult<Category>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldCategoryId, result.Entity.CategoryId);
            Assert.AreNotEqual(oldName, result.Entity.Name);
            Assert.AreNotEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void PostCategory()
        {
            var id = context.Categories.Local.Count + 1;

            Category newCategory = new Category {
                CategoryId = id,
                Name = "New Category Name"
            };

            var controller = new CategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Post(newCategory) as CreatedODataResult<Category>;
            
            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(id, result.Entity.CategoryId);
            Assert.AreNotEqual(context.Categories.Local.Count + 1, result.Entity.CategoryId);
        }

        [TestMethod]
        public void PatchCategory()
        {
            int id = new Random().Next(1, context.Categories.Local.Count + 1);

            //Recolhe dados antes da alteração
            var oldCategory = context.Categories.Find(id);
            //Guardar em variáveis para os testes
            var oldCategoryId = oldCategory.CategoryId;
            var oldName = oldCategory.Name;
            var oldModifiedDate = oldCategory.ModifiedDate;


            var delta = new Delta<Category>(typeof(Category));
            delta.TrySetPropertyValue("CategoryId", id);
            delta.TrySetPropertyValue("Name", "Modified:" + oldName);

            var controller = new CategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Patch(id, delta) as UpdatedODataResult<Category>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldCategoryId, result.Entity.CategoryId);
            Assert.AreNotEqual(oldName, result.Entity.Name);
            Assert.AreEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void DeleteCategory()
        {
            int id = new Random().Next(1, context.Categories.Local.Count + 1);

            var controller = new CategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Delete(id) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.IsNull(context.Categories.Find(id));
        }
    }
}