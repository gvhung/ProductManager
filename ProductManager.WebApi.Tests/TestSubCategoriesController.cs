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
    public class TestSubCategoriesController
    {
        private readonly TestProductManagerContext context = new TestProductManagerContext();

        public TestSubCategoriesController()
        {
            context.Initialize();
        }

        [TestMethod]
        public void GetAllSubCategories()
        {
            var controller = new SubCategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetSubCategories();

            Assert.AreEqual(context.SubCategories.Local.Count, result.Count());
        }

        [TestMethod]
        public void GetSingleSubCategory()
        {
            int id = new Random().Next(1, context.SubCategories.Local.Count + 1);

            var controller = new SubCategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetSubCategory(id).Queryable.Single();

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.SubCategoryId);
        }

        [TestMethod]
        public void PutSubCategory()
        {
            int id = new Random().Next(1, context.SubCategories.Local.Count + 1);

            //Recolhe dados antes da alteração
            var oldSubCategory = context.SubCategories.Find(id);
            //Guardar em variáveis para os testes
            var oldSubCategoryId = oldSubCategory.SubCategoryId;
            var oldName = oldSubCategory.Name;
            var oldCategoryId = oldSubCategory.CategoryId;
            var oldModifiedDate = oldSubCategory.ModifiedDate;

            //Calcular nova Categoria aleatoria diferente da anterior         
            int newCategoryId = new Random().Next(1, context.Categories.Local.Count + 1);
            while(newCategoryId == oldCategoryId)
            {
                newCategoryId = new Random().Next(1, context.Categories.Local.Count + 1);
            }

            var delta = new Delta<SubCategory>(typeof(SubCategory));
            delta.TrySetPropertyValue("SubCategoryId", id);
            delta.TrySetPropertyValue("CategoryId", newCategoryId);
            delta.TrySetPropertyValue("Name", "Modified:" + oldName);

            var controller = new SubCategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Put(id, delta) as UpdatedODataResult<SubCategory>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldSubCategoryId, result.Entity.SubCategoryId);
            Assert.AreNotEqual(oldCategoryId, result.Entity.CategoryId);
            Assert.AreNotEqual(oldName, result.Entity.Name);
            Assert.AreNotEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void PostSubCategory()
        {
            var id = context.SubCategories.Local.Count + 1;
            
            SubCategory newSubCategory = new SubCategory {
                SubCategoryId = id,
                Name = "New Category Name",
                CategoryId = new Random().Next(1, context.Categories.Local.Count + 1)
            };

            var controller = new SubCategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Post(newSubCategory) as CreatedODataResult<SubCategory>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(id, result.Entity.SubCategoryId);
            Assert.AreNotEqual(context.SubCategories.Local.Count + 1, result.Entity.SubCategoryId);
        }

        [TestMethod]
        public void PatchSubCategory()
        {
            int id = new Random().Next(1, context.SubCategories.Local.Count + 1);

            //Recolhe dados antes da alteração
            var oldSubCategory = context.SubCategories.Find(id);
            //Guardar em variáveis para os testes
            var oldSubCategoryId = oldSubCategory.SubCategoryId;
            var oldName = oldSubCategory.Name;
            var oldCategoryId = oldSubCategory.CategoryId;
            var oldModifiedDate = oldSubCategory.ModifiedDate;

            //Calcular nova Categoria aleatoria diferente da anterior         
            int newCategoryId = new Random().Next(1, context.Categories.Local.Count + 1);
            while (newCategoryId == oldCategoryId)
            {
                newCategoryId = new Random().Next(1, context.Categories.Local.Count + 1);
            }

            var delta = new Delta<SubCategory>(typeof(SubCategory));
            delta.TrySetPropertyValue("SubCategoryId", id);
            delta.TrySetPropertyValue("CategoryId", newCategoryId);
            delta.TrySetPropertyValue("Name", "Modified:" + oldName);

            var controller = new SubCategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Patch(id, delta) as UpdatedODataResult<SubCategory>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldSubCategoryId, result.Entity.SubCategoryId);
            Assert.AreNotEqual(oldCategoryId, result.Entity.CategoryId);
            Assert.AreNotEqual(oldName, result.Entity.Name);
            Assert.AreEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void DeleteSubCategory()
        {
            int id = new Random().Next(1, context.SubCategories.Local.Count + 1);

            var controller = new SubCategoriesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Delete(id) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.IsNull(context.SubCategories.Find(id));
        }
    }
}
