using ProductManager.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ProductManager.WebApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer<ProductManagerContext>(new ProductManagerInitializer());
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
