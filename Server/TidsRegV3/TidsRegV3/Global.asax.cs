using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using System.Web.Http;
using TidsRegV3.BusinessLogic.Logger;

namespace TidsRegV3
{
  public class Global : HttpApplication
  {
    protected void Application_BeginRequest()
    {
      Response.Cache.SetCacheability(HttpCacheability.NoCache);
      Response.Cache.SetExpires(DateTime.Now.AddHours(-1));
      Response.Cache.SetNoStore();
    }

    void Application_Start(object sender, EventArgs e)
    {
      // Code that runs on application startup
      GlobalConfiguration.Configure(WebApiConfig.Register);
    }
    protected void Application_Error(Object sender, EventArgs e)
    {
      Response.Redirect("ErrorPage.aspx");
    }
  }
}