﻿using Kiwipedia2._0.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Kiwipedia2._0
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Database.SetInitializer<ArticleDBContext>(new DropCreateDatabaseIfModelChanges<ArticleDBContext>());
            Database.SetInitializer<CategoryDBContext>(new DropCreateDatabaseIfModelChanges<CategoryDBContext>());
        }
    }
}
