using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.ModelBinding;
using System.Web.OData;
using System.Web.OData.Query;
using System.Web.OData.Routing;
using ProductManager.DataAccess;
using ProductManager.Entities;

namespace ProductManager.WebApi.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.OData.Builder;
    using System.Web.OData.Extensions;
    using ProductManager.Entities;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<Category>("Categories");
    config.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class CategoriesController : ODataController
    {
        private IProductManagerContext db;

        public CategoriesController()
        {
            db = new ProductManagerContext();
        }

        public CategoriesController(IProductManagerContext context)
        {
            db = context;
        }

        // GET: odata/Categories
        [EnableQuery]
        public IQueryable<Category> GetCategories()
        {
            return db.Categories;
        }

        // GET: odata/Categories(5)
        [EnableQuery]
        public SingleResult<Category> GetCategory([FromODataUri] int key)
        {
            return SingleResult.Create(db.Categories.Where(category => category.CategoryId == key));
        }

        // PUT: odata/Categories(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<Category> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = db.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }

            patch.Put(category);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // POST: odata/Categories
        public IHttpActionResult Post(Category category)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Categories.Add(category);
            db.SaveChanges();

            return Created(category);
        }

        // PATCH: odata/Categories(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<Category> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = db.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }

            patch.Patch(category);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(category);
        }

        // DELETE: odata/Categories(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            Category category = db.Categories.Find(key);
            if (category == null)
            {
                return NotFound();
            }

            db.Categories.Remove(category);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CategoryExists(int key)
        {
            return db.Categories.Count(e => e.CategoryId == key) > 0;
        }
    }
}
