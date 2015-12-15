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
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ODataClient _client;

        public CategoryController()
        {
            _client = ProductManagerODataClient.GetDefaultClient();
        }

        public CategoryController(string address)
        {
            _client = ProductManagerODataClient.GetCustomClient(address);
        }

        // GET: Category
        public async Task<ActionResult> Index()
        {
            var categories = await _client
                .For<Category>()
                .FindEntriesAsync();

            if (categories == null)
            {
                return HttpNotFound();
            }

            return View(categories);
        }

        // GET: Category/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = await _client
                .For<Category>()
                .Key(id)
                .FindEntryAsync();

            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        // GET: Category/DetailSubCategories/5
        public async Task<ActionResult> DetailSubCategories(int id)
        {
            var subCategories = await _client
                .For<SubCategory>()
                .Filter(x => x.CategoryId == id)
                .FindEntriesAsync();


            if (subCategories == null)
                return null;

            return PartialView("_DetailSubCategories", subCategories);
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "CategoryId,Name,Rowguid,ModifiedDate")] Category category)
        {
            if (ModelState.IsValid)
            {
                var newCategory = await _client
                    .For<Category>()
                    .Set(category)
                    .InsertEntryAsync();

                if (newCategory.CategoryId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = await _client
                .For<Category>()
                .Key(id)
                .FindEntryAsync();

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "CategoryId,Name,Rowguid,ModifiedDate")] Category category)
        {
            if (ModelState.IsValid)
            {

                var modifiedCategory = await _client
                    .For<Category>()
                    .Key(category.CategoryId)
                    .Set(category)
                    .UpdateEntryAsync();

                if (modifiedCategory.CategoryId != category.CategoryId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                return RedirectToAction("Index");
            }

            return View(category);
        }

        // GET: Category/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var category = await _client
                .For<Category>()
                .Key(id)
                .FindEntryAsync();

            if (category == null)
            {
                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            await _client
                .For<Category>()
                .Key(id)
                .DeleteEntryAsync();


            try
            {
                await _client
                    .For<Category>()
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