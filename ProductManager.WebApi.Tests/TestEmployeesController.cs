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
using ProductManager.Entities.Enums;

namespace ProductManager.WebApi.Tests
{
    [TestClass]
    public class TestEmployeesController
    {
        private readonly TestProductManagerContext context = new TestProductManagerContext();

        public TestEmployeesController()
        {
            context.Initialize();
        }

        [TestMethod]
        public void GetAllEmployees()
        {
            var controller = new EmployeesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetEmployees();

            Assert.AreEqual(context.Employees.Local.Count, result.Count());
        }

        [TestMethod]
        public void GetSingleEmployee()
        {
            int id = new Random().Next(1, context.Employees.Local.Count + 1);

            var controller = new EmployeesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.GetEmployee(id).Queryable.Single();

            Assert.IsNotNull(result);
            Assert.AreEqual(id, result.EmployeeId);
        }

        [TestMethod]
        public void PutEmployee()
        {
            int id = new Random().Next(1, context.Employees.Local.Count + 1);

            //Recolhe dados antes da alteração
            var oldEmployee = context.Employees.Find(id);
            //Guardar em variáveis para os testes
            var oldEmployeeId = oldEmployee.EmployeeId;
            var oldTitle = oldEmployee.Title;
            var oldFirstName = oldEmployee.FirstName;
            var oldMiddleName = oldEmployee.MiddleName;
            var oldLastName = oldEmployee.LastName;
            var oldEmail = oldEmployee.Email;
            var oldModifiedDate = oldEmployee.ModifiedDate;

            var delta = new Delta<Employee>(typeof(Employee));
            delta.TrySetPropertyValue("EmployeeId", id);
            //TODO: random enum value
            delta.TrySetPropertyValue("Title", PersonTitle.Miss);
            delta.TrySetPropertyValue("FirstName", "Modified:" + oldFirstName);
            delta.TrySetPropertyValue("MiddleName", "Modified:" + oldMiddleName);
            delta.TrySetPropertyValue("LastName", "Modified:" + oldLastName);
            delta.TrySetPropertyValue("Email", "Modified_" + oldEmail);

            var controller = new EmployeesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Put(id, delta) as UpdatedODataResult<Employee>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldEmployeeId, result.Entity.EmployeeId);
            Assert.AreNotEqual(oldTitle, result.Entity.Title);
            Assert.AreNotEqual(oldFirstName, result.Entity.FirstName);
            Assert.AreNotEqual(oldMiddleName, result.Entity.MiddleName);
            Assert.AreNotEqual(oldLastName, result.Entity.LastName);
            Assert.AreNotEqual(oldEmail, result.Entity.Email);
            Assert.AreNotEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void PostEmployee()
        {
            var id = context.Employees.Local.Count + 1;

            Employee newEmployee = new Employee {
                EmployeeId = id,
                Title = PersonTitle.Mrs,
                FirstName = "New First Name",
                MiddleName = "New Middle Name",
                LastName = "New Last Name",
                Email = "newemailadress@example.com"
            };

            var controller = new EmployeesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Post(newEmployee) as CreatedODataResult<Employee>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(id, result.Entity.EmployeeId);
            Assert.AreNotEqual(context.SubCategories.Local.Count + 1, result.Entity.EmployeeId);
        }

        [TestMethod]
        public void PatchEmployee()
        {
            int id = new Random().Next(1, context.Employees.Local.Count + 1);

            //Recolhe dados antes da alteração
            var oldEmployee = context.Employees.Find(id);
            //Guardar em variáveis para os testes
            var oldEmployeeId = oldEmployee.EmployeeId;
            var oldTitle = oldEmployee.Title;
            var oldFirstName = oldEmployee.FirstName;
            var oldMiddleName = oldEmployee.MiddleName;
            var oldLastName = oldEmployee.LastName;
            var oldEmail = oldEmployee.Email;
            var oldModifiedDate = oldEmployee.ModifiedDate;

            var delta = new Delta<Employee>(typeof(Employee));
            delta.TrySetPropertyValue("EmployeeId", id);
            //TODO: random enum value
            delta.TrySetPropertyValue("Title", PersonTitle.Mrs);
            delta.TrySetPropertyValue("FirstName", "Modified:" + oldFirstName);
            delta.TrySetPropertyValue("MiddleName", "Modified:" + oldMiddleName);
            delta.TrySetPropertyValue("LastName", "Modified:" + oldLastName);
            delta.TrySetPropertyValue("Email", "Modified_" + oldEmail);

            var controller = new EmployeesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Patch(id, delta) as UpdatedODataResult<Employee>;

            Assert.IsNotNull(result.Entity);
            Assert.AreEqual(oldEmployeeId, result.Entity.EmployeeId);
            Assert.AreNotEqual(oldTitle, result.Entity.Title);
            Assert.AreNotEqual(oldFirstName, result.Entity.FirstName);
            Assert.AreNotEqual(oldMiddleName, result.Entity.MiddleName);
            Assert.AreNotEqual(oldLastName, result.Entity.LastName);
            Assert.AreNotEqual(oldEmail, result.Entity.Email);
            Assert.AreEqual(oldModifiedDate, result.Entity.ModifiedDate);
        }

        [TestMethod]
        public void DeleteEmployee()
        {
            int id = new Random().Next(1, context.Employees.Local.Count + 1);

            var controller = new EmployeesController(context);
            controller.Configuration = new HttpConfiguration();
            WebApiConfig.Register(controller.Configuration);

            var result = controller.Delete(id) as StatusCodeResult;

            Assert.AreEqual(HttpStatusCode.NoContent, result.StatusCode);
            Assert.IsNull(context.Employees.Find(id));
        }
    }
}
