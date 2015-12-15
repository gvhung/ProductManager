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
    public class SubCategoryController : Controller
    {
        private readonly ODataClient _client;

        public SubCategoryController()
        {
            _client = ProductManagerODataClient.GetDefaultClient();
        }

        public SubCategoryController(string address)
        {
            _client = ProductManagerODataClient.GetCustomClient(address);
        }

        // GET: SubCategory
        public async Task<ActionResult> Index()
        {
            var subCategories = await _client
                .For<SubCategory>()
                .Expand(x => x.Category)
                .FindEntriesAsync();

            if (subCategories == null)
            {
                return HttpNotFound();
            }

            return View(subCategories);
        }

        // GET: SubCategory/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subCategory = await _client
                .For<SubCategory>()
                .Key(id)
                .Expand(x => x.Category)
                .FindEntryAsync();

            if (subCategory == null)
            {
                return HttpNotFound();
            }
            return View(subCategory);
        }

        // GET: Category/DetailProducts/5
        public async Task<ActionResult> DetailProducts(int id)
        {
            var products = await _client
                .For<Product>()
                .Filter(x => x.SubCategoryId == id)
                .Expand(x => x.Employee)
                .FindEntriesAsync();


            if (products == null)
                return null;

            return PartialView("_DetailProducts", products);
        }

        // GET: SubCategory/Create
        public async Task<ActionResult> Create()
        {
            var categories = await _client
               .For<Category>()
               .FindEntriesAsync();

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name");
            return View();
        }

        // POST: SubCategory/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "SubCategoryId,CategoryId,Name,Rowguid,ModifiedDate")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {
                var newSubCategory = await _client
                    .For<SubCategory>()
                    .Set(subCategory)
                    .InsertEntryAsync();

                if (newSubCategory.CategoryId == 0)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                return RedirectToAction("Index");
            }

            var categories = await _client
               .For<Category>()
               .FindEntriesAsync();

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name", subCategory.CategoryId);

            return View(subCategory);
        }

        // GET: SubCategory/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subCategory = await _client
                .For<SubCategory>()
                .Key(id)
                .FindEntryAsync();

            if (subCategory == null)
            {
                return HttpNotFound();
            }


            var categories = await _client
               .For<Category>()
               .FindEntriesAsync();

            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "Name", subCategory.CategoryId);

            return View(subCategory);
        }

        // POST: SubCategory/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "SubCategoryId,CategoryId,Name,Rowguid,ModifiedDate")] SubCategory subCategory)
        {
            if (ModelState.IsValid)
            {

                var modifiedCategory = await _client
                    .For<SubCategory>()
                    .Key(subCategory.SubCategoryId)
                    .Set(subCategory)
                    .UpdateEntryAsync();

                if (modifiedCategory.CategoryId != subCategory.CategoryId)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
                }

                return RedirectToAction("Index");
            }

            return View(subCategory);
        }

        // GET: SubCategory/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var subCategory = await _client
                .For<SubCategory>()
                .Key(id)
                .Expand(x => x.Category)
                .FindEntryAsync();

            if (subCategory == null)
            {
                return HttpNotFound();
            }

            return View(subCategory);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {

            await _client
                .For<SubCategory>()
                .Key(id)
                .DeleteEntryAsync();

            try
            {
                await _client
                    .For<SubCategory>()
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