using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using TidsRegV3.BusinessLogic.Logger;
using TidsRegV3.BusinessLogic.Modules;
using TidsRegV3.BusinessLogic.SqlCore;

namespace TidsRegV3.Controllers
{
    public class RegisterController : ApiController
    {
      private LoginModule loginModule;
      private IErrorLogger errorLogger = new ApiLogger();

      public RegisterController()
      {
        var sqlHelperBase = new SqlHelperBase(errorLogger);
        loginModule = new LoginModule(sqlHelperBase);
      }

    [HttpPost]
      public HttpResponseMessage RegisterUser(HttpRequestMessage request)
      {
        try
        {
          if (request.Content.Headers.ContentType.MediaType != "application/json")
            return new HttpResponseMessage(HttpStatusCode.UnsupportedMediaType);

          var jsonData = request.Content.ReadAsStringAsync().Result;
          var data = JObject.Parse(jsonData);
          var username = (string)data["username"];
          var password = (string)data["password"];

          if (!loginModule.RegisterUser(username, password))
          {
            return new HttpResponseMessage(HttpStatusCode.BadRequest);
          }

          return new HttpResponseMessage(HttpStatusCode.Accepted);
        }
        catch (Exception e)
        {
          return new HttpResponseMessage(HttpStatusCode.BadRequest);
        }
      }
  }
}
