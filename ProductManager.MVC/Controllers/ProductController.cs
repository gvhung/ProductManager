using ProductManager.Entities;
using ProductManager.MVC.Models;
using Simple.OData.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ProductManager.MVC.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly ODataClient _client;

        public ProductController()
        {
            _client = ProductManagerODataClient.GetDefaultClient();
        }

        public ProductController(string address)
        {
            _client = ProductManagerODataClient.GetCustomClient(address);
        }

        // GET: Product
        public async Task<ActionResult> Index()
        {
            var products = await _client
                .For<Product>()
                .Expand(e => e.Employee)
                .Expand(c => c.SubCategory)
                .FindEntriesAsync();

            return View(products);
        }

        // GET: Product/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _client
              .For<Product>()
                .Key(id)
                .Expand(e => e.Employee)
                .Expand(c => c.SubCategory)
                .FindEntryAsync();

            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Product/Create
        public async Task<ActionResult> Create()
        {
            var subCategories = await _client
                .For<SubCategory>()
                .Expand(x => x.Category)
                .FindEntriesAsync();

            var employees = await _client
              .For<Employee>()
              .FindEntriesAsync();

            ViewBag.EmployeeId = new SelectList(employees, "EmployeeId", "FullName",employees.Where(e => e.Email == User.Identity.Name).Single().EmployeeId);
            ViewBag.SubCategoryId = new SelectList(subCategories, "SubCategoryId","Name","Category.Name",0);
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "ProductId,Name,ProductNumber,StandardCost,ListPrice,SubCategoryId,EmployeeId,Rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                var newProduct = await _client
                      .For<Product>()
                      .Set(product)
                      .InsertEntryAsync();

                if (newProduct.ProductId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                return RedirectToAction("Index");
            }

            var subCategories = await _client
                .For<SubCategory>()
                .Expand(x => x.Category)
                .FindEntriesAsync();

            var employees = await _client
              .For<Employee>()
              .FindEntriesAsync();
            ViewBag.EmployeeId = new SelectList(employees, "EmployeeId", "FullName", product.EmployeeId);
            ViewBag.SubCategoryId = new SelectList(subCategories, "SubCategoryId", "Name", "Category.Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Product/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _client
             .For<Product>()
             .Key(id)
             .Expand(e => e.Employee)
             .Expand(s => s.SubCategory)
             .FindEntryAsync();

            if (product == null)
            {
                return HttpNotFound();
            }

            //Verifica se o utilizador é proprietário do produto ou é administrador
            if (!User.IsInRole("Admin") && User.Identity.Name != product.Employee.Email)
            {
                ViewBag.Message = "You don't have permission to edit this product.";
                return View("Details", product);
            }



            var subCategories = await _client
                .For<SubCategory>()
                .Expand(x => x.Category)
                .FindEntriesAsync();

            var employees = await _client
              .For<Employee>()
              .FindEntriesAsync();

            ViewBag.EmployeeId = new SelectList(employees, "EmployeeId", "FullName", product.EmployeeId);
            ViewBag.SubCategoryId = new SelectList(subCategories, "SubCategoryId", "Name", "Category.Name", product.SubCategoryId);

            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ProductId,Name,ProductNumber,StandardCost,ListPrice,SubCategoryId,EmployeeId,Rowguid,ModifiedDate")] Product product)
        {
            if (ModelState.IsValid)
            {
                var modifiedProduct = await _client
                    .For<Product>()
                    .Key(product.ProductId)
                    .Set(product)
                    .UpdateEntryAsync();

                return RedirectToAction("Index");
            }

            var subCategories = await _client
                .For<SubCategory>()
                .Expand(x => x.Category)
                .FindEntriesAsync();

            var employees = await _client
                .For<Employee>()
                .FindEntriesAsync();

            ViewBag.EmployeeId = new SelectList(employees, "EmployeeId", "FullName", product.EmployeeId);
            ViewBag.SubCategoryId = new SelectList(subCategories, "SubCategoryId", "Name", "Category.Name", product.SubCategoryId);
            return View(product);
        }

        // GET: Product/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = await _client
                .For<Product>()
                .Key(id)
                .Expand(e => e.Employee)
                .Expand(s => s.SubCategory)
                .FindEntryAsync();

            if (product == null)
            {
                return HttpNotFound();
            }

            if (!User.IsInRole("Admin") && User.Identity.Name != product.Employee.Email)
            {
                ViewBag.Message = "You don't have permission to delete this product.";
                return View("Details", product);
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            await _client
                .For<Product>()
                .Key(id)
                .DeleteEntryAsync();

            try
            {
                await _client
                    .For<Product>()
                    .Key(id)
                    .FindEntryAsync();
            }
            catch (Simple.OData.Client.WebRequestException)
            {
                return RedirectToAction("Index");
            }

            return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
        }
    }
}