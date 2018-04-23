using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Text;
using System.Threading;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Modules;
using TidsRegV3.BusinessLogic.SqlCore;
using TidsRegV3.BusinessLogic.Validators;

namespace TidsRegV3.Controllers
{
  public class BasicAuthAttribute : AuthorizationFilterAttribute
  {
    private IErrorLogger errorLogger;

    public BasicAuthAttribute()
    {
      this.errorLogger = new ApiLogger();
    }

    /// <summary>Calls when a process requests authorization.</summary>
    /// <param name="actionContext">The action context, which encapsulates information for using <see cref="T:System.Web.Http.Filters.AuthorizationFilterAttribute" />.</param>
    public override void OnAuthorization(HttpActionContext actionContext)
    {
      if (actionContext.Request.Headers.Authorization == null)
      {
        actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
      }
      else
      {
        try
        {


          var auth = Encoding.UTF8
            .GetString(Convert.FromBase64String(actionContext.Request.Headers.Authorization.Parameter)).Split(':');

          auth.ToList().ForEach(x => BasicValidator.ValidateAsNonSpacedString(x, errorLogger));

          var username = auth[0];
          var password = auth[1];

          var login = new LoginModule(new SqlHelperBase(errorLogger));

          if (login.UserLogin(username, password))
          {
            Thread.CurrentPrincipal = new GenericPrincipal(new GenericIdentity(username), null);
          }
          else
          {
            actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
          }
        }
        catch (Exception e)
        {
          actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized);
        }
      }
    }
  }
}