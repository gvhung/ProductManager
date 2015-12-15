using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.OData.Builder;
using System.Web.OData.Extensions;
using System.Web.OData.Batch;
using Microsoft.OData.Edm;
using ProductManager.Entities;

namespace ProductManager.WebApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            config.MapHttpAttributeRoutes();

            //OData routes
            config.MapODataServiceRoute("odata", "odata", GetEdmModel(), new DefaultODataBatchHandler(GlobalConfiguration.DefaultServer));
            config.EnsureInitialized();
        }

        //Método para criar um EdmModel
        private static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder { Namespace = "ProdManager.WebApi" };
            builder.EntitySet<Employee>("Employees");
            builder.EntitySet<Category>("Categories");
            builder.EntitySet<SubCategory>("SubCategories");
            builder.EntitySet<Product>("Products");

            var edmModel = builder.GetEdmModel();

            return edmModel;
        }
    }
}
